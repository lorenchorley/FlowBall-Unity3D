
using System;

namespace NoFlo { 

    public class BasePort : EventEmitter {
        /*
 constructor: (options) ->
     @handleOptions options
     @sockets = []
     @node = null
     @name = null
     */

        public BasePort() {
            throw new NotImplementedException();
        }

        /*
   handleOptions: (options) ->
     options = {} unless options
     options.datatype = 'all' unless options.datatype
     options.required = false if options.required is undefined

     options.datatype = 'int' if options.datatype is 'integer'
     if validTypes.indexOf(options.datatype) is -1
       throw new Error "Invalid port datatype '#{options.datatype}' specified, valid are #{validTypes.join(', ')}"

     if options.type and options.type.indexOf('/') is -1
       throw new Error "Invalid port type '#{options.type}' specified. Should be URL or MIME type"

     @options = options
     */

        public void handleOptions() {
            throw new NotImplementedException();
        }

        /*
   getId: ->
     unless @node and @name
       return 'Port'
     "#{@node} #{@name.toUpperCase()}"
     */

        public void getId() {
            throw new NotImplementedException();
        }

        /*
   getDataType: -> @options.datatype
   */

        public void getDataType() {
            throw new NotImplementedException();
        }

        /*
   getDescription: -> @options.description
   */

        public void getDescription() {
            throw new NotImplementedException();
        }

        /*
   attach: (socket, index = null) ->
     if not @isAddressable() or index is null
       index = @sockets.length
     @sockets[index] = socket
     @attachSocket socket, index
     if @isAddressable()
       @emit 'attach', socket, index
       return
     @emit 'attach', socket
     */

        public void attach() {
            throw new NotImplementedException();
        }

        /*
   attachSocket: ->
   */

        public void attachSocket() {
            throw new NotImplementedException();
        }

        /*
   detach: (socket) ->
     index = @sockets.indexOf socket
     if index is -1
       return
     @sockets[index] = undefined
     if @isAddressable()
       @emit 'detach', socket, index
       return
     @emit 'detach', socket
     */

        public void detach() {
            throw new NotImplementedException();
        }

        /*
   isAddressable: ->
     return true if @options.addressable
     false
     */

        public void isAddressable() {
            throw new NotImplementedException();
        }

        /*
   isBuffered: ->
     return true if @options.buffered
     false
     */

        public void isBuffered() {
            throw new NotImplementedException();
        }

        /*
   isRequired: ->
     return true if @options.required
     false
     */

        public void isRequired() {
            throw new NotImplementedException();
        }

        /*
   isAttached: (socketId = null) ->
     if @isAddressable() and socketId isnt null
       return true if @sockets[socketId]
       return false
     return true if @sockets.length
     false
     */

        public void isAttached() {
            throw new NotImplementedException();
        }

        /*
   listAttached: ->
     attached = []
     for socket, idx in @sockets
       continue unless socket
       attached.push idx
     attached
     */

        public void listAttached() {
            throw new NotImplementedException();
        }

        /*
   isConnected: (socketId = null) ->
     if @isAddressable()
       throw new Error "#{@getId()}: Socket ID required" if socketId is null
       throw new Error "#{@getId()}: Socket #{socketId} not available" unless @sockets[socketId]
       return @sockets[socketId].isConnected()

     connected = false
     @sockets.forEach (socket) =>
       return unless socket
       if socket.isConnected()
         connected = true
     return connected
     */

        public void isConnected() {
            throw new NotImplementedException();
        }

        /*
   canAttach: -> true
          */
        public void canAttach() {
            throw new NotImplementedException();
        }

    }

}