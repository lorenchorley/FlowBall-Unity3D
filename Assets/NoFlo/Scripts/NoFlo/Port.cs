
using System;

namespace NoFlo { 

    public class Port : EventEmitter {
        /*
         description: ''
  required: true
  constructor: (@type) ->
    platform.deprecated 'noflo.Port is deprecated. Please port to noflo.InPort/noflo.OutPort'
    @type = 'all' unless @type
    @type = 'int' if @type is 'integer'
    @sockets = []
    @from = null
    @node = null
    @name = null
    */

        public Port() {
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
  getDataType: -> @type
  */

        public void getDataType() {
            throw new NotImplementedException();
        }

        /*
  getDescription: -> @description
  */

        public void getDescription() {
            throw new NotImplementedException();
        }

        /*
  attach: (socket) ->
    @sockets.push socket
    @attachSocket socket
    */

        public void attach() {
            throw new NotImplementedException();
        }

        /*
  attachSocket: (socket, localId = null) ->
    @emit "attach", socket, localId

    @from = socket.from
    socket.setMaxListeners 0 if socket.setMaxListeners
    socket.on "connect", =>
      @emit "connect", socket, localId
    socket.on "begingroup", (group) =>
      @emit "begingroup", group, localId
    socket.on "data", (data) =>
      @emit "data", data, localId
    socket.on "endgroup", (group) =>
      @emit "endgroup", group, localId
    socket.on "disconnect", =>
      @emit "disconnect", socket, localId
      */

        public void attachSocket() {
            throw new NotImplementedException();
        }

        /*
  connect: ->
    if @sockets.length is 0
      throw new Error "#{@getId()}: No connections available"
    socket.connect() for socket in @sockets
    */

        public void connect() {
            throw new NotImplementedException();
        }

        /*
  beginGroup: (group) ->
    if @sockets.length is 0
      throw new Error "#{@getId()}: No connections available"

    @sockets.forEach (socket) ->
      return socket.beginGroup group if socket.isConnected()
      socket.once 'connect', ->
        socket.beginGroup group
      do socket.connect
      */

        public void beginGroup() {
            throw new NotImplementedException();
        }

        /*
  send: (data) ->
    if @sockets.length is 0
      throw new Error "#{@getId()}: No connections available"

    @sockets.forEach (socket) ->
      return socket.send data if socket.isConnected()
      socket.once 'connect', ->
        socket.send data
      do socket.connect
      */

        public void send() {
            throw new NotImplementedException();
        }

        /*
  endGroup: ->
    if @sockets.length is 0
      throw new Error "#{@getId()}: No connections available"
    socket.endGroup() for socket in @sockets
    */

        public void endGroup() {
            throw new NotImplementedException();
        }

        /*
  disconnect: ->
    if @sockets.length is 0
      throw new Error "#{@getId()}: No connections available"
    socket.disconnect() for socket in @sockets
    */

        public void disconnect() {
            throw new NotImplementedException();
        }

        /*
  detach: (socket) ->
    return if @sockets.length is 0
    socket = @sockets[0] unless socket
    index = @sockets.indexOf socket
    return if index is -1
    if @isAddressable()
      @sockets[index] = undefined
      @emit 'detach', socket, index
      return
    @sockets.splice index, 1
    @emit "detach", socket
    */

        public void detach() {
            throw new NotImplementedException();
        }

        /*
  isConnected: ->
    connected = false
    @sockets.forEach (socket) =>
      if socket.isConnected()
        connected = true
    connected
    */

        public void isConnected() {
            throw new NotImplementedException();
        }

        /*
  isAddressable: -> false
  */

        public void isAddressable() {
            throw new NotImplementedException();
        }

        /*
  isRequired: -> @required
  */

        public void isRequired() {
            throw new NotImplementedException();
        }

        /*

  isAttached: ->
    return true if @sockets.length > 0
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
  canAttach: -> true
  */
        public void canAttach() {
            throw new NotImplementedException();
        }

    }

}
 