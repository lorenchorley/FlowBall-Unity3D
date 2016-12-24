
using System;

namespace NoFlo { 

    public class OutPorts : Ports {
        /*
  model: OutPort

  connect: (name, socketId) ->
    throw new Error "Port #{name} not available" unless @ports[name]
    @ports[name].connect socketId*/

        public void connect() {
            throw new NotImplementedException();
        }

        /*
  beginGroup: (name, group, socketId) ->
    throw new Error "Port #{name} not available" unless @ports[name]
    @ports[name].beginGroup group, socketId
    */

        public void beginGroup() {
            throw new NotImplementedException();
        }

        /*
  send: (name, data, socketId) ->
    throw new Error "Port #{name} not available" unless @ports[name]
    @ports[name].send data, socketId
    */

        public void send() {
            throw new NotImplementedException();
        }

        /*
  endGroup: (name, socketId) ->
    throw new Error "Port #{name} not available" unless @ports[name]
    @ports[name].endGroup socketId
    */

        public void endGroup() {
            throw new NotImplementedException();
        }

        /*
  disconnect: (name, socketId) ->
    throw new Error "Port #{name} not available" unless @ports[name]
    @ports[name].disconnect socketId
    */

        public void disconnect() {
            throw new NotImplementedException();
        }

    }

}