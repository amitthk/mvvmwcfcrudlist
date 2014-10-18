using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MvvmWcfCrudList.Service;
using System.ServiceModel.Description;

namespace MvvmWcfCrudList.Common
{
    class BootStrapper
    {
        private static BootStrapper _instance;
        private static ServiceHost _host;

        private static TodoServiceClient _todoServiceClient;

        public static BootStrapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BootStrapper();
                }
                return (_instance);
            }
        }

        public TodoServiceClient todoServiceClient { get { return (_todoServiceClient); } }

        private BootStrapper()
        {
            _host = new ServiceHost(typeof(TodoService), new Uri("net.pipe://localhost"));
            _host.AddServiceEndpoint(typeof(MvvmWcfCrudList.Service.ITodoService), new NetNamedPipeBinding(), "MvvmWcfCrudListSvc");

            //In case we are hosting there service in this application. It will pick the settings from app.config
           // _host = new ServiceHost(typeof(MvvmWcfCrudList.Service.TodoService));                        
        }


        public void Bootstrap(App app, System.Windows.StartupEventArgs e)
        {
            //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            //smb.HttpGetEnabled = true;
            //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            //_host.Description.Behaviors.Add(smb);


            //Start server in new thread
            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem(state =>
                {
                    _host.Open();
                });
            }
            catch (Exception exc)
            {
                _host.Abort();
                throw (exc);
            }

            //Start client
            _todoServiceClient = new TodoServiceClient("NetNamedPipeBinding_ITodoService");

        }

        public void ShutDown(App app, System.Windows.ExitEventArgs e)
        {
            try
            {
                _host.Close();
            }
            catch (Exception exc)
            {
                _host.Abort();
                throw (exc);
            }
        }
    }
}
