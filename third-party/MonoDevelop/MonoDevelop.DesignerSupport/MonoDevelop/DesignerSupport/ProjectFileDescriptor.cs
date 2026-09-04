using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using MonoDevelop.Components.PropertyGrid.PropertyEditors;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport
{
	internal class ProjectFileDescriptor : CustomDescriptor
	{
		private abstract class StandardStringsConverter : TypeConverter
		{
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (!(sourceType == typeof(string)))
				{
					return base.CanConvertFrom(context, sourceType);
				}
				return true;
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (!(destinationType == typeof(string)))
				{
					return base.CanConvertTo(context, destinationType);
				}
				return true;
			}

			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value != null && value is string)
				{
					return value;
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (value != null && destinationType == typeof(string))
				{
					return value;
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				return new StandardValuesCollection(GetStandardStrings(context));
			}

			public abstract ICollection GetStandardStrings(ITypeDescriptorContext context);
		}

		[StandardValuesSeparator("--")]
		private class BuildActionStringsConverter : StandardStringsConverter
		{
			public override ICollection GetStandardStrings(ITypeDescriptorContext context)
			{
				ProjectFileDescriptor projectFileDescriptor = ((context != null) ? (context.Instance as ProjectFileDescriptor) : null);
				if (projectFileDescriptor != null && projectFileDescriptor.file != null && projectFileDescriptor.file.Project != null)
				{
					return projectFileDescriptor.file.Project.GetBuildActions();
				}
				return new string[3] { "Content", "None", "Compile" };
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string);
			}

			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				return (string)value;
			}

			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (!IsValid(context, value))
				{
					throw new FormatException("Invalid build target name");
				}
				return (string)value;
			}

			public override bool IsValid(ITypeDescriptorContext context, object value)
			{
				if (!(value is string))
				{
					return false;
				}
				string text = (string)value;
				if (string.IsNullOrEmpty(text) || !char.IsLetter(text[0]))
				{
					return false;
				}
				for (int i = 1; i < text.Length; i++)
				{
					char c = text[i];
					if (!char.IsLetterOrDigit(c) && c != '_')
					{
						return false;
					}
				}
				return true;
			}

			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}
		}

		private ProjectFile file;

		[LocalizedCategory("Misc")]
		[LocalizedDescription("Name of the file.")]
		[LocalizedDisplayName("Name")]
		public string Name => System.IO.Path.GetFileName(file.Name);

		[LocalizedCategory("Misc")]
		[LocalizedDisplayName("Path")]
		[LocalizedDescription("Full path of the file.")]
		public string Path => file.FilePath;

		[LocalizedCategory("Misc")]
		[LocalizedDisplayName("Type")]
		[LocalizedDescription("Type of the file.")]
		public string FileType
		{
			get
			{
				string mimeTypeForUri = DesktopService.GetMimeTypeForUri(file.Name);
				return DesktopService.GetMimeTypeDescription(mimeTypeForUri);
			}
		}

		[LocalizedCategory("Build")]
		[LocalizedDisplayName("Build action")]
		[LocalizedDescription("Action to perform when building this file.")]
		[TypeConverter(typeof(BuildActionStringsConverter))]
		public string BuildAction
		{
			get
			{
				return file.BuildAction;
			}
			set
			{
				file.BuildAction = value;
			}
		}

		[LocalizedCategory("Build")]
		[LocalizedDisplayName("Resource ID")]
		[LocalizedDescription("Identifier of the embedded resource.")]
		public string ResourceId
		{
			get
			{
				return file.ResourceId;
			}
			set
			{
				file.ResourceId = value;
			}
		}

		[LocalizedDescription("Whether to copy the file to the project's output directory when the project is built.")]
		[LocalizedCategory("Build")]
		[LocalizedDisplayName("Copy to output directory")]
		public FileCopyMode CopyToOutputDirectory
		{
			get
			{
				return file.CopyToOutputDirectory;
			}
			set
			{
				file.CopyToOutputDirectory = value;
			}
		}

		[LocalizedDisplayName("Custom Tool")]
		[LocalizedCategory("Build")]
		[LocalizedDescription("The ID of a custom code generator.")]
		public string Generator
		{
			get
			{
				return file.Generator;
			}
			set
			{
				file.Generator = value;
			}
		}

		[LocalizedCategory("Build")]
		[LocalizedDisplayName("Custom Tool Namespace")]
		[LocalizedDescription("Overrides the namespace in which the custom code generator should generate code.")]
		public string CustomToolNamespace
		{
			get
			{
				return file.CustomToolNamespace;
			}
			set
			{
				file.CustomToolNamespace = value;
			}
		}

		public ProjectFileDescriptor(ProjectFile file)
		{
			this.file = file;
		}

		protected override bool IsReadOnly(string propertyName)
		{
			return false;
		}
	}
}
