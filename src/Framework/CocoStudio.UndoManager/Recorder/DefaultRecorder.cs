using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CocoStudio.Basic;

namespace CocoStudio.UndoManager.Recorder
{
	// Token: 0x02000014 RID: 20
	public class DefaultRecorder : BaseRecorder
	{
		// Token: 0x0600008C RID: 140 RVA: 0x0000326F File Offset: 0x0000146F
		public DefaultRecorder(INotifyStateChanged objectItem, string taskGroupName = null) : base(objectItem, taskGroupName)
		{
			this.Initialize();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000328E File Offset: 0x0000148E
		private void Initialize()
		{
			this.AnalyzeObject();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003298 File Offset: 0x00001498
		private void AnalyzeObject()
		{
			bool flag = false;
			PropertyInfo[] properties = this.objectItem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.GetCustomAttributes(typeof(UndoPropertyAttribute), true).Length > 0)
				{
					if (propertyInfo.CanRead)
					{
						object value = propertyInfo.GetValue(this.objectItem, DefaultRecorder.EmptyArray);
						if (propertyInfo.CanWrite)
						{
							this.oldValueList[propertyInfo.Name] = value;
							flag = true;
						}
						if (value is INotifyCollectionChanged)
						{
							((INotifyCollectionChanged)value).CollectionChanged += this.ObjectItem_CollectionChanged;
						}
					}
				}
			}
			if (flag)
			{
				this.objectItem.PropertyChanged += this.ObjectItem_PropertyChanged;
				this.objectItem.StateChanged += this.ObjectItem_StateChanged;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000033B4 File Offset: 0x000015B4
		private bool CanIgnore()
		{
			return BaseRecorder.IsUndoing || !base.IsAutoRecord;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000033DC File Offset: 0x000015DC
		private void ObjectItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			try
			{
				if (base.IsAutoRecord && this.ContainsProperty(e.PropertyName))
				{
					this.PropertyChangedHandle(sender, e);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("ObjectItem_PropertyChanged exception.", exception);
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003440 File Offset: 0x00001640
		private bool ContainsProperty(string propertyName)
		{
			return this.oldValueList.ContainsKey(propertyName);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003460 File Offset: 0x00001660
		private void ObjectItem_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			try
			{
				if (!this.CanIgnore())
				{
					this.CollectionChangedHandle(sender, e);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("CollectionChangedHandle exception.", exception);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000034B4 File Offset: 0x000016B4
		private void ObjectItem_StateChanged(object sender, StateChangedEventArgs e)
		{
			try
			{
				if (!this.CanIgnore() && this.ContainsProperty(e.PropertyName))
				{
					this.StateChangedHandle(sender, e);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("PropertyChangedHandle exception.", exception);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003518 File Offset: 0x00001718
		private void PropertyChangedHandle(object sender, PropertyChangedEventArgs e)
		{
			object obj = null;
			PropertyInfo propertyInfo = null;
			if (this.IsCollectionProperty(sender, e.PropertyName, out propertyInfo, out obj))
			{
				this.UpdateCollectionChangedRegister(e.PropertyName, obj as INotifyCollectionChanged);
			}
			else
			{
				this.oldValueList[e.PropertyName] = obj;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000356C File Offset: 0x0000176C
		private void CollectionChangedHandle(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (sender is IInsertableList)
			{
				this.RegisterIListChanges(sender, e);
			}
			else if (DefaultRecorder.IsCollection(sender))
			{
				this.RegisterICollectionChanges(sender, e);
			}
			else
			{
				LogConfig.Logger.Error("The Class that implements INotifyCollectionChanged does not Implement ICollection<T>, IList or IList<T>");
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000035C4 File Offset: 0x000017C4
		private void StateChangedHandle(object sender, StateChangedEventArgs e)
		{
			object obj = null;
			PropertyInfo property = null;
			if (this.IsCollectionProperty(sender, e.PropertyName, out property, out obj))
			{
				this.UpdateCollectionChangedRegister(e.PropertyName, obj as INotifyCollectionChanged);
			}
			else
			{
				object oldValue = this.oldValueList[e.PropertyName];
				if (!this.IsValueEquals(obj, oldValue))
				{
					PropertyUndoTask undoTask = this.CreatePropertyUndoTask(e, property, oldValue, obj);
					this.AddTask(undoTask);
				}
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003640 File Offset: 0x00001840
		private bool IsCollectionProperty(object sender, string propertyName, out PropertyInfo property, out object newValue)
		{
			property = sender.GetType().GetProperty(propertyName);
			newValue = property.GetValue(sender, DefaultRecorder.EmptyArray);
			return newValue is INotifyCollectionChanged;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003688 File Offset: 0x00001888
		private void UpdateCollectionChangedRegister(string propertyName, INotifyCollectionChanged newValue)
		{
			object obj = this.oldValueList[propertyName];
			INotifyCollectionChanged notifyCollectionChanged = obj as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= this.ObjectItem_CollectionChanged;
			}
			if (newValue != null)
			{
				newValue.CollectionChanged += this.ObjectItem_CollectionChanged;
			}
			this.oldValueList[propertyName] = newValue;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003ADC File Offset: 0x00001CDC
		private void RegisterICollectionChanges(object sender, NotifyCollectionChangedEventArgs e)
		{
			MethodInfo add = sender.GetType().GetMethod("Add");
			MethodInfo remove = sender.GetType().GetMethod("Remove");
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				CollectionUndoTask undoTask = new CollectionUndoTask(this, e.NewItems, e.OldItems, delegate(object param)
				{
					foreach (object obj in e.NewItems)
					{
						add.Invoke(sender, new object[]
						{
							obj
						});
					}
				}, delegate(object param)
				{
					foreach (object obj in e.NewItems)
					{
						remove.Invoke(sender, new object[]
						{
							obj
						});
					}
				}, null);
				this.AddTask(undoTask);
				break;
			}
			case NotifyCollectionChangedAction.Remove:
			{
				CollectionUndoTask undoTask2 = new CollectionUndoTask(this, e.NewItems, e.OldItems, delegate(object param)
				{
					foreach (object obj in e.OldItems)
					{
						remove.Invoke(sender, new object[]
						{
							obj
						});
					}
				}, delegate(object param)
				{
					foreach (object obj in e.OldItems)
					{
						add.Invoke(sender, new object[]
						{
							obj
						});
					}
				}, null);
				this.AddTask(undoTask2);
				break;
			}
			case NotifyCollectionChangedAction.Replace:
			{
				CollectionUndoTask undoTask3 = new CollectionUndoTask(this, e.NewItems, e.OldItems, delegate(object param)
				{
					foreach (object obj in e.OldItems)
					{
						remove.Invoke(sender, new object[]
						{
							obj
						});
					}
					foreach (object obj in e.NewItems)
					{
						add.Invoke(sender, new object[]
						{
							obj
						});
					}
				}, delegate(object param)
				{
					foreach (object obj in e.NewItems)
					{
						remove.Invoke(sender, new object[]
						{
							obj
						});
					}
					foreach (object obj in e.OldItems)
					{
						add.Invoke(sender, new object[]
						{
							obj
						});
					}
				}, null);
				this.AddTask(undoTask3);
				break;
			}
			case NotifyCollectionChangedAction.Move:
				throw new NotSupportedException("You can only Move object in IList or IList<T> not Collection<T>");
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004100 File Offset: 0x00002300
		private void RegisterIListChanges(object sender, NotifyCollectionChangedEventArgs e)
		{
			PropertyInfo indexer = sender.GetType().GetProperty("Item");
			MethodInfo insert = sender.GetType().GetMethod("Insert");
			MethodInfo removeAt = sender.GetType().GetMethod("RemoveAt");
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				CollectionUndoTask undoTask = new CollectionUndoTask(this, e.NewItems, e.OldItems, delegate(object param)
				{
					for (int i = e.NewItems.Count - 1; i >= 0; i--)
					{
						insert.Invoke(sender, new object[]
						{
							e.NewStartingIndex,
							e.NewItems[i]
						});
					}
				}, delegate(object param)
				{
					for (int i = 0; i < e.NewItems.Count; i++)
					{
						if (indexer.GetValue(sender, new object[]
						{
							e.NewStartingIndex
						}) != e.NewItems[i])
						{
							Debugger.Break();
						}
						removeAt.Invoke(sender, new object[]
						{
							e.NewStartingIndex
						});
					}
				}, null);
				this.AddTask(undoTask);
				break;
			}
			case NotifyCollectionChangedAction.Remove:
			{
				CollectionUndoTask undoTask2 = new CollectionUndoTask(this, e.NewItems, e.OldItems, delegate(object param)
				{
					for (int i = 0; i < e.OldItems.Count; i++)
					{
						removeAt.Invoke(sender, new object[]
						{
							e.OldStartingIndex
						});
					}
				}, delegate(object param)
				{
					for (int i = e.OldItems.Count - 1; i >= 0; i--)
					{
						insert.Invoke(sender, new object[]
						{
							e.OldStartingIndex,
							e.OldItems[i]
						});
					}
				}, null);
				this.AddTask(undoTask2);
				break;
			}
			case NotifyCollectionChangedAction.Replace:
			{
				CollectionUndoTask undoTask3 = new CollectionUndoTask(this, e.NewItems, e.OldItems, delegate(object param)
				{
					for (int i = e.NewItems.Count - 1; i >= 0; i--)
					{
						indexer.SetValue(sender, e.NewItems[i], new object[]
						{
							i + e.NewStartingIndex
						});
					}
				}, delegate(object param)
				{
					for (int i = 0; i < e.NewItems.Count; i++)
					{
						indexer.SetValue(sender, e.OldItems[i], new object[]
						{
							i + e.NewStartingIndex
						});
					}
				}, null);
				this.AddTask(undoTask3);
				break;
			}
			case NotifyCollectionChangedAction.Move:
			{
				CollectionUndoTask undoTask4 = new CollectionUndoTask(this, e.NewItems, e.OldItems, delegate(object param)
				{
					for (int i = 0; i < e.OldItems.Count; i++)
					{
						removeAt.Invoke(sender, new object[]
						{
							e.OldStartingIndex
						});
					}
					for (int i = e.NewItems.Count - 1; i >= 0; i--)
					{
						insert.Invoke(sender, new object[]
						{
							e.NewStartingIndex,
							e.NewItems[i]
						});
					}
				}, delegate(object param)
				{
					for (int i = 0; i < e.NewItems.Count; i++)
					{
						removeAt.Invoke(sender, new object[]
						{
							e.NewStartingIndex
						});
					}
					for (int i = e.NewItems.Count - 1; i >= 0; i--)
					{
						insert.Invoke(sender, new object[]
						{
							e.OldStartingIndex,
							e.OldItems[i]
						});
					}
				}, null);
				this.AddTask(undoTask4);
				break;
			}
			case NotifyCollectionChangedAction.Reset:
				LogConfig.Logger.Error("No NotifyCollectionChangedAction.Reset");
				break;
			default:
				LogConfig.Logger.Error("No default shuld exist");
				break;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004350 File Offset: 0x00002550
		private static bool IsList(object sender)
		{
			bool result;
			if (sender is IList)
			{
				result = true;
			}
			else
			{
				Queue<Type> queue = new Queue<Type>(sender.GetType().GetInterfaces());
				while (queue.Count != 0)
				{
					Type type = queue.Dequeue();
					foreach (Type item in type.GetInterfaces())
					{
						queue.Enqueue(item);
					}
					if (type.IsGenericType)
					{
						Type genericTypeDefinition = typeof(IList<object>).GetGenericTypeDefinition();
						Type genericTypeDefinition2 = type.GetGenericTypeDefinition();
						if (genericTypeDefinition == genericTypeDefinition2)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004420 File Offset: 0x00002620
		private static bool IsCollection(object sender)
		{
			Queue<Type> queue = new Queue<Type>(sender.GetType().GetInterfaces());
			while (queue.Count != 0)
			{
				Type type = queue.Dequeue();
				foreach (Type item in type.GetInterfaces())
				{
					queue.Enqueue(item);
				}
				if (type.IsGenericType)
				{
					Type genericTypeDefinition = typeof(ICollection<object>).GetGenericTypeDefinition();
					Type genericTypeDefinition2 = type.GetGenericTypeDefinition();
					if (genericTypeDefinition == genericTypeDefinition2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000044D4 File Offset: 0x000026D4
		protected PropertyUndoTask CreatePropertyUndoTask(StateChangedEventArgs e, PropertyInfo property, object oldValue, object newValue)
		{
			PropertyUndoTask result;
			if (e.IsProvideValue)
			{
				this.oldValueList[e.PropertyName] = e.NewValue;
				result = new PropertyUndoTask(this, this.objectItem, property, e.OldValue, e.NewValue);
			}
			else
			{
				result = new PropertyUndoTask(this, this.objectItem, property, oldValue, newValue);
			}
			return result;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000453C File Offset: 0x0000273C
		private void AddTask(UndoTask undoTask)
		{
			if (TaskServiceSingleton.Instance.Enable)
			{
				base.AddRecord(undoTask);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004564 File Offset: 0x00002764
		private bool IsValueEquals(object newValue, object oldValue)
		{
			bool result;
			if (newValue != null)
			{
				result = newValue.Equals(oldValue);
			}
			else
			{
				result = (oldValue == null);
			}
			return result;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004598 File Offset: 0x00002798
		protected List<UndoTask> CollectChangedProperty(bool isCreateRecorder)
		{
			List<UndoTask> list = new List<UndoTask>();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type type = this.objectItem.GetType();
			foreach (KeyValuePair<string, object> keyValuePair in this.oldValueList)
			{
				PropertyInfo property = type.GetProperty(keyValuePair.Key);
				object value = property.GetValue(this.objectItem, DefaultRecorder.EmptyArray);
				if (!this.IsValueEquals(value, keyValuePair.Value))
				{
					if (value is INotifyCollectionChanged)
					{
						this.UpdateCollectionChangedRegister(keyValuePair.Key, value as INotifyCollectionChanged);
					}
					else if (isCreateRecorder)
					{
						list.Add(new PropertyUndoTask(this, this.objectItem, property, keyValuePair.Value, value));
					}
					dictionary.Add(keyValuePair.Key, value);
				}
			}
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				this.oldValueList[keyValuePair.Key] = keyValuePair.Value;
			}
			return list;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004710 File Offset: 0x00002910
		protected override void OnStart(bool isCreateRecorder)
		{
			this.objectItem.PropertyChanged += this.ObjectItem_PropertyChanged;
			List<UndoTask> list = this.CollectChangedProperty(isCreateRecorder);
			foreach (UndoTask undoTask in list)
			{
				base.AddRecord(undoTask);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004788 File Offset: 0x00002988
		protected override void OnStop(bool isUpdateOldValues = false)
		{
			this.objectItem.PropertyChanged -= this.ObjectItem_PropertyChanged;
			if (isUpdateOldValues)
			{
				this.UpdateCachedValue();
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000047C0 File Offset: 0x000029C0
		~DefaultRecorder()
		{
			this.Dispose();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000047F4 File Offset: 0x000029F4
		public override void Dispose()
		{
			if (this.objectItem != null)
			{
				this.objectItem.PropertyChanged -= this.ObjectItem_PropertyChanged;
				this.objectItem.StateChanged -= this.ObjectItem_StateChanged;
				this.oldValueList.Clear();
				this.objectItem = null;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004864 File Offset: 0x00002A64
		public void UpdateCachedValue(string propertyName, object value)
		{
			if (this.ContainsProperty(propertyName))
			{
				this.oldValueList[propertyName] = value;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004890 File Offset: 0x00002A90
		public void UpdateCachedValue()
		{
			foreach (KeyValuePair<string, object> keyValuePair in this.oldValueList.ToList<KeyValuePair<string, object>>())
			{
				PropertyInfo property = this.objectItem.GetType().GetProperty(keyValuePair.Key);
				object value = property.GetValue(this.objectItem, DefaultRecorder.EmptyArray);
				this.oldValueList[keyValuePair.Key] = value;
			}
		}

		// Token: 0x04000020 RID: 32
		internal static readonly object[] EmptyArray = new object[0];

		// Token: 0x04000021 RID: 33
		private readonly Dictionary<string, object> oldValueList = new Dictionary<string, object>();
	}
}
