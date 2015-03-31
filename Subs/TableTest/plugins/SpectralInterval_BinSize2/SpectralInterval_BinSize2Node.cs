#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "Interval_BinSize2", Category = "Spectral", Help = "Basic template with 2 dimensional spread with bin size input", Tags = "")]
	#endregion PluginInfo
	public class SpectralInterval_BinSize2Node : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		public ISpread<ISpread<double>> FInput;
		
		[Input("Intervals", DefaultValue = 1.0)]
		public ISpread<double> FInterval;

		[Output("Output")]
		public ISpread<double> FOutput;

		[Import()]
		public ILogger Flogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = FInput.SliceCount;

			for (int i = 0; i < FInput.SliceCount; i++) {
				FOutput[i] = 0;
				for (int j = 0; j < FInput[i].SliceCount; j++)
					FOutput[i] += FInput[i][j];
			}
		}
	}
}
