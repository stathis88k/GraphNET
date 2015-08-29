﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraphApp.GraphService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GraphService.IGraphService")]
    public interface IGraphService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGraphService/UpdateDB", ReplyAction="http://tempuri.org/IGraphService/UpdateDBResponse")]
        void UpdateDB();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGraphService/UpdateDB", ReplyAction="http://tempuri.org/IGraphService/UpdateDBResponse")]
        System.Threading.Tasks.Task UpdateDBAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGraphService/ShortestPath", ReplyAction="http://tempuri.org/IGraphService/ShortestPathResponse")]
        void ShortestPath(string a, string b);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGraphService/ShortestPath", ReplyAction="http://tempuri.org/IGraphService/ShortestPathResponse")]
        System.Threading.Tasks.Task ShortestPathAsync(string a, string b);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGraphServiceChannel : GraphApp.GraphService.IGraphService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GraphServiceClient : System.ServiceModel.ClientBase<GraphApp.GraphService.IGraphService>, GraphApp.GraphService.IGraphService {
        
        public GraphServiceClient() {
        }
        
        public GraphServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GraphServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GraphServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GraphServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void UpdateDB() {
            base.Channel.UpdateDB();
        }
        
        public System.Threading.Tasks.Task UpdateDBAsync() {
            return base.Channel.UpdateDBAsync();
        }
        
        public void ShortestPath(string a, string b) {
            base.Channel.ShortestPath(a, b);
        }
        
        public System.Threading.Tasks.Task ShortestPathAsync(string a, string b) {
            return base.Channel.ShortestPathAsync(a, b);
        }
    }
}
