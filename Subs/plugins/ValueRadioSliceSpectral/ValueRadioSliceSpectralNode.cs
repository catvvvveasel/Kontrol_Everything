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
	[PluginInfo(Name = "RadioSliceSpectral", Category = "Value", Help = "Gives the Slice Index of incoming Bangs", Tags = "Radio")]
	#endregion PluginInfo
	public class ValueRadioSliceSpectralNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		public ISpread<ISpread<double>> FInput;

		[Input("DefValue")]
		public ISpread<double> FDefault;

		[Input("Set")]
		public ISpread<bool> FSet;


		[Output("Output")]
		public ISpread<double> FOutput;


		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = FInput.SliceCount;


			for (int i = 0; i < FInput.SliceCount; i++)
			{
			if (FSet[i] == true) 
				{
				FOutput[i] = FDefault[i];
				} 
			else
				for (int j = 0; j < FInput[i].SliceCount; j++)
				{

					if (FInput[i][j] >= 1f) 
					{
						FOutput[i] = j;

					}

				}
			}
		}
	}
}
