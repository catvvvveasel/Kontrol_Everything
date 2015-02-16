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
	[PluginInfo(Name = "MutExFlipFlop", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueMutExFlipFlopNode : IPluginEvaluate
	{
	#region fields & pins
		[Input("Input")]
		public ISpread<double> FInput;

		[Output("Output")]
		public ISpread<float> FOutput;


		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = SpreadMax;


			for (int i = 0; i < FInput.SliceCount; i++)
			{

	FOutput[i]= 0f;
					if (FInput[i] >= 1f) 
					{
					FOutput[i]= 1f;
						break;
					}
					
				

			}
		}
	}
}
