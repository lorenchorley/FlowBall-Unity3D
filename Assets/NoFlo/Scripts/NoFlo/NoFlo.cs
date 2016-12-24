
using System;
using System.Collections.Generic;
using System.IO;

namespace NoFlo { 

    public class NoFlo {

        public delegate void LoadCreateCallback(string err, Network network);
        public delegate void SaveCallback(string file); // TODO

        public bool isBrowser() {
            return false;
        }

        public bool isNodeJS() {
            return false;
        }

        public bool isUnity() {
            return true;
        }

        /*
        # ## Network instantiation
        #
        # This function handles instantiation of NoFlo networks from a Graph object. It creates
        # the network, and then starts execution by sending the Initial Information Packets.
        #
        # noflo.createNetwork(someGraph, function (err, network) {
        # console.log('Network is now running!');
        #     });
        #
        # It is also possible to instantiate a Network but delay its execution by giving the
        # third `delay` parameter. In this case you will have to handle connecting the graph and
        # sending of IIPs manually.
        #
        # noflo.createNetwork(someGraph, function (err, network) {
        #if (err) {
        # throw err;
        #       }
        # network.connect(function (err) {
        # network.start();
        # console.log('Network is now running!');
        #       })
        #     }, true);
        exports.createNetwork = (graph, callback, options) ->
            unless typeof options is 'object'
            options =
                delay: options
            unless typeof callback is 'function'
            callback = (err) ->
                throw err if err

            network = new exports.Network graph, options

            networkReady = (network) ->
        # Send IIPs
            network.start (err) ->
                return callback err if err
                callback null, network

        # Ensure components are loaded before continuing
            network.loader.listComponents (err) ->
            return callback err if err
        # Empty network, no need to connect it up
            return networkReady network if graph.nodes.length is 0

        # In case of delayed execution we don't wire it up
            if options.delay
                callback null, network
                return

        # Wire the network up and start execution
            network.connect (err) ->
                return callback err if err
                networkReady network

            network
        */
        public Network createNetwork(Graph graph, LoadCreateCallback callback, Dictionary<string, object> options) {
            Network network = new Network(graph, options);
            network.loader.listComponents(() => {

            });
            throw new NotImplementedException();
        }

        /*
        # ### Starting a network from a file
        #
        # It is also possible to start a NoFlo network by giving it a path to a `.json` or `.fbp` network
        # definition file.
        #
        #     noflo.loadFile('somefile.json', function (err, network) {
        #       if (err) {
        #         throw err;
        #       }
        #       console.log('Network is now running!');
        #     });
        exports.loadFile = (file, options, callback) ->
          unless callback
            callback = options
            baseDir = null

          if callback and typeof options isnt 'object'
            options =
              baseDir: options

          exports.graph.loadFile file, (err, net) ->
            return callback err if err
            net.baseDir = options.baseDir if options.baseDir
            exports.createNetwork net, callback, options
        */
        public void loadFile(string file, Dictionary<string, string> options, LoadCreateCallback callback) {
            throw new NotImplementedException();
        }

        /*
        # ### Saving a network definition
        #
        # NoFlo graph files can be saved back into the filesystem with this method.
        exports.saveFile = (graph, file, callback) ->
          exports.graph.save file, -> callback file
        */
        public void saveFile(Graph graph, string file, SaveCallback callback) {
            throw new NotImplementedException();
        }

    }

}
 