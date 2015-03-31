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
	[PluginInfo(Name = "IntervalBinSize", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueIntervalBinSizeNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		public ISpread<double> FInput;
		
		[Input("Intervals", DefaultValue = 1.0)]
		public ISpread<double> FIntervals;
		
		[Input("IntervalsBinSize", DefaultValue = 1.0)]
		public ISpread<int> FIntervalsBinSize;

		[Output("Indices")]
		public ISpread<double> FIndices;
		
		[Output("IndicesGlobal")]
		public ISpread<double> FIndicesGlobal;
		
		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		
		public void calculate(){
			
			for (int i = 0; i < FIntervalsBinSize.SliceCount; i++){
				
				int offSet=0;
				
				if(i>0){
					
					offSet=FIntervalsBinSize[i-1];
					
				}

				int intervalLength=FIntervalsBinSize[i];
				int result=-1;
				int resultGlobal =-1;
				
				
				for(int k=0; k < intervalLength;k++){
					
					int index = k+offSet;
					
					
					 if( layOnInterval(FInput[0],FIntervals[index],FIntervals[index+1])){
					 	
					 	result=k;
					 	resultGlobal=index;
					 	
					 }
					
				}
				
				
				FIndices[i] = result;
				FIndicesGlobal[i] = resultGlobal;

			//FLogger.Log(LogType.Debug, "hi tty!");
			
			}
			
		}
		public bool layOnInterval(double value,double cueIn , double cueOut){
			
			bool result=false;
			
			if(value>= cueIn && value <cueOut){
				
				
				result=true;
			}
			
			return result;
			
		}
		
		public void Evaluate(int SpreadMax)
		{
			
			FIndices.SliceCount = FIntervalsBinSize.SliceCount;
			FIndicesGlobal.SliceCount = FIntervalsBinSize.SliceCount;
			
			calculate();
			
			
			
		}
	}
}
