#region usings
using System;
using System.ComponentModel.Composition;
using System.Linq;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "FileEnumerator", Category = "Enumerations", Version = "Dynamic", Help = "Basic template with dynamic custom enumeration", Tags = "", AutoEvaluate = true)]
	#endregion PluginInfo
	public class DynamicEnumerationsFileEnumeratorNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", EnumName = "AllFileEnumerators")]
        public IDiffSpread<EnumEntry> FInput;
		
		[Output("Name")]
        public ISpread<string> FNameOutput;
		
		[Import()]
		public ILogger Flogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			//if (FInput.IsChanged)
			{
				FNameOutput[0] = RootHolder.SAssetRoot + FInput[0].Name;
			}
		}
	}
	
	public static class RootHolder
	{
		public static string SAssetRoot;
	}
	
	#region PluginInfo
	[PluginInfo(Name = "SetFileEnumerator", Category = "Enumerations", Version = "Dynamic", Help = "Basic template with dynamic custom enumeration", Tags = "", AutoEvaluate = true)]
	#endregion PluginInfo
	public class DynamicEnumerationsSetFileEnumeratorNode : IPluginEvaluate
	{

		[Input("UpdateEnum", IsBang = true)]
		public ISpread<bool> FChangeEnum;
		
		[Input("Asset Root")]
		public ISpread<string> FRootInput;

		[Input("Enum Entries")]
		public ISpread<string> FEnumStrings;

		[Import()]
		public ILogger Flogger;

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (FChangeEnum[0]) 
			{
				RootHolder.SAssetRoot = FRootInput[0];
				if(FEnumStrings.SliceCount > 0)
				{
					EnumManager.UpdateEnum("AllFileEnumerators", FEnumStrings[0], FEnumStrings.ToArray());
				}
				
				Flogger.Log(LogType.Debug, FRootInput[0]);
			}
		}
	}
}
