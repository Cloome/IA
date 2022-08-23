using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace CleoPydza
{
    class Connexion
    {
        //test
        #region attributes
        private TcpClient client;
        private StreamReader fluxEntrant;
        private StreamWriter fluxSortant;

        #endregion

        #region property
        public StreamReader FluxEntrant { get => fluxEntrant; }
        public StreamWriter FluxSortant { get => fluxSortant; set => fluxSortant = value; }


        #endregion


        #region methodes
        public void ConnexionServeur()
        {
            client = new TcpClient("127.0.0.1", 1234);
        }

        public void CreationFlux()
        {
            fluxEntrant = new StreamReader(client.GetStream());
            fluxSortant = new StreamWriter(client.GetStream());
            fluxSortant.AutoFlush = true;
        }
        #endregion

        public void Stop()
        {
            this.client.Close();
        }
    }
}
