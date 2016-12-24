
using System;

namespace NoFlo { 

    public class AsyncComponent : Component {
        /*

  constructor: (@inPortName="in", @outPortName="out", @errPortName="error") ->
    platform.deprecated 'noflo.AsyncComponent is deprecated. Please port to Process API'

    unless @inPorts[@inPortName]
      throw new Error "no inPort named '#{@inPortName}'"
    unless @outPorts[@outPortName]
      throw new Error "no outPort named '#{@outPortName}'"

    @load = 0
    @q = []
    @errorGroups = []

    @outPorts.load = new port.Port()

    @inPorts[@inPortName].on "begingroup", (group) =>
      return @q.push { name: "begingroup", data: group } if @load > 0
      @errorGroups.push group
      @outPorts[@outPortName].beginGroup group

    @inPorts[@inPortName].on "endgroup", =>
      return @q.push { name: "endgroup" } if @load > 0
      @errorGroups.pop()
      @outPorts[@outPortName].endGroup()

    @inPorts[@inPortName].on "disconnect", =>
      return @q.push { name: "disconnect" } if @load > 0
      @outPorts[@outPortName].disconnect()
      @errorGroups = []
      @outPorts.load.disconnect() if @outPorts.load.isAttached()

    @inPorts[@inPortName].on "data", (data) =>
      return @q.push { name: "data", data: data } if @q.length > 0
      @processData data
      */

        public AsyncComponent() {
            throw new NotImplementedException();
        }

        /*
  processData: (data) ->
    @incrementLoad()
    @doAsync data, (err) =>
      @error err, @errorGroups, @errPortName if err
      @decrementLoad()
      */

        public void processData() {
            throw new NotImplementedException();
        }

        /*
  incrementLoad: ->
    @load++
    @outPorts.load.send @load if @outPorts.load.isAttached()
    @outPorts.load.disconnect() if @outPorts.load.isAttached()
    */

        public void incrementLoad() {
            throw new NotImplementedException();
        }

        /*
  doAsync: (data, callback) ->
    callback new Error "AsyncComponents must implement doAsync"
    */

        public void doAsync() {
            throw new NotImplementedException();
        }

        /*
  decrementLoad: ->
    throw new Error "load cannot be negative" if @load == 0
    @load--
    @outPorts.load.send @load if @outPorts.load.isAttached()
    @outPorts.load.disconnect() if @outPorts.load.isAttached()
    if typeof process isnt 'undefined' and process.execPath and process.execPath.indexOf('node') isnt -1
      # nextTick is faster than setTimeout on Node.js
      process.nextTick => @processQueue()
    else
      setTimeout =>
        do @processQueue
      , 0
      */

        public void decrementLoad() {
            throw new NotImplementedException();
        }

        /*
  processQueue: ->
    if @load > 0
      return
    processedData = false
    while @q.length > 0
      event = @q[0]
      switch event.name
        when "begingroup"
          return if processedData
          @outPorts[@outPortName].beginGroup event.data
          @errorGroups.push event.data
          @q.shift()
        when "endgroup"
          return if processedData
          @outPorts[@outPortName].endGroup()
          @errorGroups.pop()
          @q.shift()
        when "disconnect"
          return if processedData
          @outPorts[@outPortName].disconnect()
          @outPorts.load.disconnect() if @outPorts.load.isAttached()
          @errorGroups = []
          @q.shift()
        when "data"
          @processData event.data
          @q.shift()
          processedData = true
*/

        public void processQueue() {
            throw new NotImplementedException();
        }

        /*
  shutdown: ->
    @q = []
    @errorGroups = []
    */

        public void shutdown() {
            throw new NotImplementedException();
        }

        /*
  # Old-style error function because of legacy ports
  error: (e, groups = [], errorPort = 'error') =>
    if @outPorts[errorPort] and (@outPorts[errorPort].isAttached() or not @outPorts[errorPort].isRequired())
      @outPorts[errorPort].beginGroup group for group in groups
      @outPorts[errorPort].send e
      @outPorts[errorPort].endGroup() for group in groups
      @outPorts[errorPort].disconnect()
      return
    throw e
    */
        public void error() {
            throw new NotImplementedException();
        }

    }

}