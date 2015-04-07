#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using VVVV.Nodes.Table;
using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes.Table
{
	#region PluginInfo
	[PluginInfo(Name = "TableNames3", Category = "String", Help = "Basic template with one string in/out", Tags = "")]
	#endregion PluginInfo
	public class StringTableNames3Node : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultString = "hello c#")]
		public ISpread<string> FInput;
int spreadMax = tables.SliceCount;
		[Output("Output")]
		public ISpread<string> FOutput;

		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = SpreadMax;

			for (int i = 0; i < SpreadMax; i++)
				FOutput[i] = FInput[i].Replace("c#", "vvvv");

			//FLogger.Log(LogType.Debug, "Logging to Renderer (TTY)");
		}
	}
}
