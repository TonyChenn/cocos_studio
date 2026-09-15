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
	public class DefaultRecorder : BaseRecorder
	{
		public DefaultRecorder(INotifyStateChanged objectItem, string taskGroupName = null) : base(objectItem, taskGroupName)
		{
			this.Initialize();
		}

		private void Initialize()
		{
			this.AnalyzeObject();
		}

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

		private bool CanIgnore()
		{
			return BaseRecorder.IsUndoing || !base.IsAutoRecord;
		}

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

		private bool ContainsProperty(string propertyName)
		{
			return this.oldValueList.ContainsKey(propertyName);
		}

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

		private bool IsCollectionProperty(object sender, string propertyName, out PropertyInfo property, out object newValue)
		{
			property = sender.GetType().GetProperty(propertyName);
			newValue = property.GetValue(sender, DefaultRecorder.EmptyArray);
			return newValue is INotifyCollectionChanged;
		}

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

		private void AddTask(UndoTask undoTask)
		{
			if (TaskServiceSingleton.Instance.Enable)
			{
				base.AddRecord(undoTask);
			}
		}

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

		protected override void OnStart(bool isCreateRecorder)
		{
			this.objectItem.PropertyChanged += this.ObjectItem_PropertyChanged;
			List<UndoTask> list = this.CollectChangedProperty(isCreateRecorder);
			foreach (UndoTask undoTask in list)
			{
				base.AddRecord(undoTask);
			}
		}

		protected override void OnStop(bool isUpdateOldValues = false)
		{
			this.objectItem.PropertyChanged -= this.ObjectItem_PropertyChanged;
			if (isUpdateOldValues)
			{
				this.UpdateCachedValue();
			}
		}

		~DefaultRecorder()
		{
			this.Dispose();
		}

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

		public void UpdateCachedValue(string propertyName, object value)
		{
			if (this.ContainsProperty(propertyName))
			{
				this.oldValueList[propertyName] = value;
			}
		}

		public void UpdateCachedValue()
		{
			foreach (KeyValuePair<string, object> keyValuePair in this.oldValueList.ToList<KeyValuePair<string, object>>())
			{
				PropertyInfo property = this.objectItem.GetType().GetProperty(keyValuePair.Key);
				object value = property.GetValue(this.objectItem, DefaultRecorder.EmptyArray);
				this.oldValueList[keyValuePair.Key] = value;
			}
		}

		internal static readonly object[] EmptyArray = new object[0];

		private readonly Dictionary<string, object> oldValueList = new Dictionary<string, object>();
	}
}
