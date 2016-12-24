﻿
using System;

namespace NoFlo {

    public class Component : EventEmitter {

        public string componentName;

        /*
description: ''
  icon: null

  constructor: (options) ->
    options = {} unless options
    options.inPorts = {} unless options.inPorts
    if options.inPorts instanceof ports.InPorts
      @inPorts = options.inPorts
    else
      @inPorts = new ports.InPorts options.inPorts

    options.outPorts = {} unless options.outPorts
    if options.outPorts instanceof ports.OutPorts
      @outPorts = options.outPorts
    else
      @outPorts = new ports.OutPorts options.outPorts

    @icon = options.icon if options.icon
    @description = options.description if options.description

    @started = false
    @load = 0
    @ordered = options.ordered ? false
    @autoOrdering = options.autoOrdering ? null
    @outputQ = []
    @activateOnInput = options.activateOnInput ? true
    @forwardBrackets = in: ['out', 'error']
    @bracketCounter = {}

    if 'forwardBrackets' of options
      @forwardBrackets = options.forwardBrackets

    if typeof options.process is 'function'
      @process options.process
      */

        public Component() {
            throw new NotImplementedException();
        }

        /*
  getDescription: -> @description
  */

        public void getDescription() {
            throw new NotImplementedException();
        }

        /*
  isReady: -> true
  */

        public void isReady() {
            throw new NotImplementedException();
        }

        /*
  isSubgraph: -> false
  */

        public void isSubgraph() {
            throw new NotImplementedException();
        }

        /*
  setIcon: (@icon) ->
    @emit 'icon', @icon
    */

        public void setIcon() {
            throw new NotImplementedException();
        }

        /*
  getIcon: -> @icon
  */

        public void getIcon() {
            throw new NotImplementedException();
        }

        /*
  error: (e, groups = [], errorPort = 'error', scope = null) =>
    if @outPorts[errorPort] and (@outPorts[errorPort].isAttached() or not @outPorts[errorPort].isRequired())
      @outPorts[errorPort].openBracket group, scope: scope for group in groups
      @outPorts[errorPort].data e, scope: scope
      @outPorts[errorPort].closeBracket group, scope: scope for group in groups
      # @outPorts[errorPort].disconnect()
      return
    throw e
    */

        public void error() {
            throw new NotImplementedException();
        }

        /*
  shutdown: ->
    @started = false
    */

        public void shutdown() {
            throw new NotImplementedException();
        }

        /*
  # The startup function performs initialization for the component.
  start: ->
    @started = true
    @started
    */

        public void start() {
            throw new NotImplementedException();
        }

        /*
  isStarted: -> @started
  */

        public void isStarted() {
            throw new NotImplementedException();
        }

        /*
  # Ensures braket forwarding map is correct for the existing ports
  prepareForwarding: ->
    for inPort, outPorts of @forwardBrackets
      unless inPort of @inPorts.ports
        delete @forwardBrackets[inPort]
        continue
      tmp = []
      for outPort in outPorts
        tmp.push outPort if outPort of @outPorts.ports
      if tmp.length is 0
        delete @forwardBrackets[inPort]
      else
        @forwardBrackets[inPort] = tmp
        @bracketCounter[inPort] = 0
        */

        public void prepareForwarding() {
            throw new NotImplementedException();
        }

        /*
  # Sets process handler function
  process: (handle) ->
    unless typeof handle is 'function'
      throw new Error "Process handler must be a function"
    unless @inPorts
      throw new Error "Component ports must be defined before process function"
    @prepareForwarding()
    @handle = handle
    for name, port of @inPorts.ports
      do (name, port) =>
        port.name = name unless port.name
        port.on 'ip', (ip) =>
          @handleIP ip, port
    @
    */

        public void process() {
            throw new NotImplementedException();
        }

        /*
  # Handles an incoming IP object
  handleIP: (ip, port) ->
    if ip.type is 'openBracket'
      @autoOrdering = true if @autoOrdering is null
      @bracketCounter[port.name]++
    if port.name of @forwardBrackets and
    (ip.type is 'openBracket' or ip.type is 'closeBracket')
      # Bracket forwarding
      outputEntry =
        __resolved: true
        __forwarded: true
        __type: ip.type
        __scope: ip.scope
      for outPort in @forwardBrackets[port.name]
        outputEntry[outPort] = [] unless outPort of outputEntry
        outputEntry[outPort].push ip
      if ip.scope?
        port.scopedBuffer[ip.scope].pop()
      else
        port.buffer.pop()
      @outputQ.push outputEntry
      @processOutputQueue()
      return
    return unless port.options.triggering
    result = {}
    input = new ProcessInput @inPorts, ip, @, port, result
    output = new ProcessOutput @outPorts, ip, @, result
    @load++
    @handle input, output, -> output.done()
    */

        public void handleIP() {
            throw new NotImplementedException();
        }

        /*
  processOutputQueue: ->
    while @outputQ.length > 0
      result = @outputQ[0]
      break unless result.__resolved
      for port, ips of result
        continue if port.indexOf('__') is 0
        continue unless @outPorts.ports[port].isAttached()
        for ip in ips
          @bracketCounter[port]-- if ip.type is 'closeBracket'
          @outPorts[port].sendIP ip
      @outputQ.shift()
    bracketsClosed = true
    for name, port of @outPorts.ports
      if @bracketCounter[port] isnt 0
        bracketsClosed = false
        break
    @autoOrdering = null if bracketsClosed and @autoOrdering is true

    */

        public void processOutputQueue() {
            throw new NotImplementedException();
        }

    }

    public class ProcessInput {

    /*
class ProcessInput
constructor: (@ports, @ip, @nodeInstance, @port, @result) ->
@scope = @ip.scope
@buffer = new PortBuffer(@)
*/

        public ProcessInput() {
            throw new NotImplementedException();
        }

        /*
  # Sets component state to `activated`
  activate: ->
    @result.__resolved = false
    if @nodeInstance.ordered or @nodeInstance.autoOrdering
      @nodeInstance.outputQ.push @result
      */

        public void activate() {
            throw new NotImplementedException();
        }

        /*
  # Returns true if a port (or ports joined by logical AND) has a new IP
  # Passing a validation callback as a last argument allows more selective
  # checking of packets.
  has: (args...) ->
    args = ['in'] unless args.length
    if typeof args[args.length - 1] is 'function'
      validate = args.pop()
      for port in args
        return false unless @ports[port].has @scope, validate
      return true
    res = true
    res and= @ports[port].ready @scope for port in args
    res
    */

        public void has() {
            throw new NotImplementedException();
        }

        /*
  # Fetches IP object(s) for port(s)
  get: (args...) ->
    args = ['in'] unless args.length
    if (@nodeInstance.ordered or @nodeInstance.autoOrdering) and
    @nodeInstance.activateOnInput and
    not ('__resolved' of @result)
      @activate()
    res = (@ports[port].get @scope for port in args)
    if args.length is 1 then res[0] else res
    */

        public void get() {
            throw new NotImplementedException();
        }

        /*
  # Fetches `data` property of IP object(s) for given port(s)
  getData: (args...) ->
    args = ['in'] unless args.length

    datas = []
    for port in args
      packet = @get port
      unless packet?
        # we add the null packet to the array so when getting
        # multiple ports, if one is null we still return it
        # so the indexes are correct.
        datas.push packet
        continue

      until packet.type is 'data'
        packet = @get port

      packet = packet?.data ? undefined
      datas.push packet

      # check if there is any other `data` IPs
      unless (@buffer.find port, (ip) -> ip.type is 'data').length > 0
        @buffer.set port, []

    return datas.pop() if args.length is 1
    datas
    */

        public void getData() {
            throw new NotImplementedException();
        }

        /*
  hasStream: (port) ->
    buffer = @buffer.get port
    return false if buffer.length is 0
    # check if we have everything until "disconnect"
    received = 0
    for packet in buffer
      if packet.type is 'openBracket'
        ++received
      else if packet.type is 'closeBracket'
        --received
    received is 0
    */

        public void hasStream() {
            throw new NotImplementedException();
        }

        /*
  getStream: (port, withoutConnectAndDisconnect = false) ->
    buf = @buffer.get port
    @buffer.filter port, (ip) -> false
    if withoutConnectAndDisconnect
      buf = buf.slice 1
      buf.pop()
    buf
    */

        public void getStream() {
            throw new NotImplementedException();
        }

    }

    public class PortBuffer {

        /*
class PortBuffer
  constructor: (@context) ->
  */

        public PortBuffer() {
            throw new NotImplementedException();
        }

        /*
  set: (name, buffer) ->
    if name? and typeof name isnt 'string'
      buffer = name
      name = null

    if @context.scope?
      if name?
        @context.ports[name].scopedBuffer[@context.scope] = buffer
        return @context.ports[name].scopedBuffer[@context.scope]
      @context.port.scopedBuffer[@context.scope] = buffer
      return @context.port.scopedBuffer[@context.scope]

    if name?
      @context.ports[name].buffer = buffer
      return @context.ports[name].buffer

    @context.port.buffer = buffer
    return @context.port.buffer
    */

        public void set() {
            throw new NotImplementedException();
        }

        /*
  # Get a buffer (scoped or not) for a given port
  # if name is optional, use the current port
  get: (name = null) ->
    if @context.scope?
      if name?
        return @context.ports[name].scopedBuffer[@context.scope]
      return @context.port.scopedBuffer[@context.scope]

    if name?
      return @context.ports[name].buffer
    return @context.port.buffer
    */

        public void get() {
            throw new NotImplementedException();
        }

        /*
  # Find packets matching a callback and return them without modifying the buffer
  find: (name, cb) ->
    b = @get name
    b.filter cb
    */

        public void find() {
            throw new NotImplementedException();
        }

        /*
  # Find packets and modify the original buffer
  # cb is a function with 2 arguments (ip, index)
  filter: (name, cb) ->
    if name? and typeof name isnt 'string'
      cb = name
      name = null

    b = @get name
    b = b.filter cb

    @set name, b
    */

        public void filter() {
            throw new NotImplementedException();
        }

    }

    public class ProcessOutput {

        /*
class ProcessOutput
  constructor: (@ports, @ip, @nodeInstance, @result) ->
    @scope = @ip.scope
    */

        public ProcessOutput() {
            throw new NotImplementedException();
        }

        /*
  # Sets component state to `activated`
  activate: ->
    @result.__resolved = false
    if @nodeInstance.ordered or @nodeInstance.autoOrdering
      @nodeInstance.outputQ.push @result
      */

        public void activate() {
            throw new NotImplementedException();
        }

        /*
  # Checks if a value is an Error
  isError: (err) ->
    err instanceof Error or
    Array.isArray(err) and err.length > 0 and err[0] instanceof Error
    */

        public void isError() {
            throw new NotImplementedException();
        }

        /*
  # Sends an error object
  error: (err) ->
    multiple = Array.isArray err
    err = [err] unless multiple
    if 'error' of @ports and
    (@ports.error.isAttached() or not @ports.error.isRequired())
      @sendIP 'error', new IP 'openBracket' if multiple
      @sendIP 'error', e for e in err
      @sendIP 'error', new IP 'closeBracket' if multiple
    else
      throw e for e in err
      */

        public void error() {
            throw new NotImplementedException();
        }

        /*
  # Sends a single IP object to a port
  sendIP: (port, packet) ->
    if typeof packet isnt 'object' or IP.types.indexOf(packet.type) is -1
      ip = new IP 'data', packet
    else
      ip = packet
    ip.scope = @scope if @scope isnt null and ip.scope is null
    if @nodeInstance.ordered or @nodeInstance.autoOrdering
      @result[port] = [] unless port of @result
      @result[port].push ip
    else
      @nodeInstance.outPorts[port].sendIP ip
      */

        public void sendIP() {
            throw new NotImplementedException();
        }

        /*
  # Sends packets for each port as a key in the map
  # or sends Error or a list of Errors if passed such
  send: (outputMap) ->
    if (@nodeInstance.ordered or @nodeInstance.autoOrdering) and
    not ('__resolved' of @result)
      @activate()
    return @error outputMap if @isError outputMap

    componentPorts = []
    mapIsInPorts = false
    for port in Object.keys @ports.ports
      componentPorts.push port if port isnt 'error' and port isnt 'ports' and port isnt '_callbacks'
      if not mapIsInPorts and outputMap? and typeof outputMap is 'object' and Object.keys(outputMap).indexOf(port) isnt -1
        mapIsInPorts = true

    if componentPorts.length is 1 and not mapIsInPorts
      @sendIP componentPorts[0], outputMap
      return

    for port, packet of outputMap
      @sendIP port, packet
      */

        public void send() {
            throw new NotImplementedException();
        }

        /*
  # Sends the argument via `send()` and marks activation as `done()`
  sendDone: (outputMap) ->
    @send outputMap
    @done()
    */

        public void sendDone() {
            throw new NotImplementedException();
        }

        /*
  # Makes a map-style component pass a result value to `out`
  # keeping all IP metadata received from `in`,
  # or modifying it if `options` is provided
  pass: (data, options = {}) ->
    unless 'out' of @ports
      throw new Error 'output.pass() requires port "out" to be present'
    for key, val of options
      @ip[key] = val
    @ip.data = data
    @sendIP 'out', @ip
    @done()
    */

        public void pass() {
            throw new NotImplementedException();
        }

        /*
  # Finishes process activation gracefully
  done: (error) ->
    @error error if error
    if @nodeInstance.ordered or @nodeInstance.autoOrdering
      @result.__resolved = true
      @nodeInstance.processOutputQueue()
    @nodeInstance.load--
    */
        public void done() {
            throw new NotImplementedException();
        }

    }

}
 
 