using System;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;

namespace CocoStudio.LuaBinding
{
	public class LuaConfiguration : ProjectConfiguration
	{
		[ItemProperty("MainFile", DefaultValue = "main.lua")]
		private string _MainFile = "main.lua";

		[ItemProperty("InterpreterArgs")]
		private string _InterpreterArgs = string.Empty;

		[ItemProperty("LangVersion", DefaultValue = LangVersion.Lua)]
		private LangVersion _LangVersion;

		public string MainFile
		{
			get
			{
				return _MainFile;
			}
			set
			{
				_MainFile = value ?? string.Empty;
			}
		}

		public string InterpreterArguments
		{
			get
			{
				return _InterpreterArgs;
			}
			set
			{
				_InterpreterArgs = value ?? string.Empty;
			}
		}

		public LangVersion LangVersion
		{
			get
			{
				return _LangVersion;
			}
			set
			{
				_LangVersion = value;
			}
		}

		public LuaConfiguration()
		{
		}

		public LuaConfiguration(string name)
		{
			base.Name = name;
		}

		public override void CopyFrom(ItemConfiguration config)
		{
			if (!(config is LuaConfiguration luaConfiguration))
			{
				throw new ArgumentException("Not a Lua configuration", "config");
			}
			base.CopyFrom(config);
			_MainFile = luaConfiguration._MainFile;
			_InterpreterArgs = luaConfiguration._InterpreterArgs;
			_LangVersion = luaConfiguration._LangVersion;
		}
	}
}
