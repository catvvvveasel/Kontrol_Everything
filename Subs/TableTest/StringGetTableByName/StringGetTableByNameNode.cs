using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using VVVV.PluginInterfaces.V2;
using VVVV.Core.Logging;
using VVVV.Nodes.Table;

namespace VVVV.Nodes.Table
{


	public class GetTableByString : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultString = "hello c#")]
        public ISpread<string> FInput;

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
