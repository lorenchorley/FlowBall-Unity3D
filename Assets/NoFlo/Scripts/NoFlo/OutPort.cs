using System;

namespace NoFlo { 

    public class OutPort : BasePort {
        /*
  constructor: (options) ->
    @cache = {}
    super options
    */

        public OutPort() {
            throw new NotImplementedException();
        }

        /*
      attach: (socket, index = null) ->
        super socket, index
        if @isCaching() and @cache[index]?
          @send @cache[index], index
              */

        public void attach() {
            throw new NotImplementedException();
        }

        /*
connect: (socketId = null) ->
  sockets = @getSockets socketId
  @checkRequired sockets
  for socket in sockets
    continue unless socket
    socket.connect()
    */

        public void connect() {
            throw new NotImplementedException();
        }

        /*
  beginGroup: (group, socketId = null) ->
    sockets = @getSockets socketId
    @checkRequired sockets
    sockets.forEach (socket) ->
      return unless socket
      return socket.beginGroup group
      */

        public void beginGroup() {
            throw new NotImplementedException();
        }

        /*
  send: (data, socketId = null) ->
    sockets = @getSockets socketId
    @checkRequired sockets
    if @isCaching() and data isnt @cache[socketId]
      @cache[socketId] = data
    sockets.forEach (socket) ->
      return unless socket
      return socket.send data
      */

        public void send() {
            throw new NotImplementedException();
        }

        /*
  endGroup: (socketId = null) ->
    sockets = @getSockets socketId
    @checkRequired sockets
    for socket in sockets
      continue unless socket
      socket.endGroup()
      */

        public void endGroup() {
            throw new NotImplementedException();
        }

        /*
  disconnect: (socketId = null) ->
    sockets = @getSockets socketId
    @checkRequired sockets
    for socket in sockets
      continue unless socket
      socket.disconnect()
      */

        public void disconnect() {
            throw new NotImplementedException();
        }

        /*
  sendIP: (type, data, options, socketId, autoConnect = true) ->
    if IP.isIP type
      ip = type
      socketId = ip.index
    else
      ip = new IP type, data, options
    sockets = @getSockets socketId
    @checkRequired sockets
    if @isCaching() and data isnt @cache[socketId]?.data
      @cache[socketId] = ip
    pristine = true
    for socket in sockets
      continue unless socket
      if pristine
        socket.post ip, autoConnect
        pristine = false
      else
        ip = ip.clone() if ip.clonable
        socket.post ip, autoConnect
    @
    */

        public void sendIP() {
            throw new NotImplementedException();
        }

        /*
  openBracket: (data = null, options = {}, socketId = null) ->
    @sendIP 'openBracket', data, options, socketId
    */

        public void openBracket() {
            throw new NotImplementedException();
        }

        /*
  data: (data, options = {}, socketId = null) ->
    @sendIP 'data', data, options, socketId
    */

        public void data() {
            throw new NotImplementedException();
        }

        /*
  closeBracket: (data = null, options = {}, socketId = null) ->
    @sendIP 'closeBracket', data, options, socketId
    */

        public void closeBracket() {
            throw new NotImplementedException();
        }

        /*
  checkRequired: (sockets) ->
    if sockets.length is 0 and @isRequired()
      throw new Error "#{@getId()}: No connections available"
      */

        public void checkRequired() {
            throw new NotImplementedException();
        }

        /*
  getSockets: (socketId) ->
    # Addressable sockets affect only one connection at time
    if @isAddressable()
      throw new Error "#{@getId()} Socket ID required" if socketId is null
      return [] unless @sockets[socketId]
      return [@sockets[socketId]]
    # Regular sockets affect all outbound connections
    @sockets
    */

        public void getSockets() {
            throw new NotImplementedException();
        }

        /*
  isCaching: ->
    return true if @options.caching
    false
    */

        public void isCaching() {
            throw new NotImplementedException();
        }

    }

}