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
	[PluginInfo(Name = "Interval_BinSize", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueInterval_BinSizeNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		public ISpread<double> FInput;
		
		[Input("Intervals", DefaultValue = 1.0)]
		public ISpread<double> FInterval;
		
		[Input("BinSize", DefaultValue = 1.0)]
		public ISpread<int> FBin;		

		[Output("Output")]
		public ISpread<double> FOutput;

		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = FBin.SliceCount;
		int offset=0;
			for (int i = 0; i < FBin.SliceCount; i++) //loop trough the number of intervals 
			{
		
				if (i==(int)0){offset =0;}
				else
				offset+=FBin[i-1];
				
				for (int j = 0; j < FBin[i]-1; j++) // for each range loop through as many times as there are intervals
				{	FOutput[i]=-1;
					
								
				if ((FInput[i]<= FInterval[j+1+offset])&&(FInput[i]>= FInterval[j+offset])	)
					{
					FOutput[i]=j ; 	
						break;
					}
				

				}
					
				}
			
//				for (int i = 0; i < FBin.SliceCount; i++)
//			{
//			FOutput[i]=-1; 
//				for (int j = 0; j < FBin[i]; j++) 
//				{	
//					FOutput[i]=j; //+FBin[i] ;
//					if (FInput[i]> FInterval[j+FBin[i]]) {break;}			
//				//	if (FInput[i]< FInterval[j+FBin[i]])	// if input is 
//				//		{FOutput[i]=j ; }
//					
//
//				}
//					
//			}			
//				
				
			}

			//FLogger.Log(LogType.Debug, "hi tty!");
		}
	}

