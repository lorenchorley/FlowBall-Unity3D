﻿
using System;
using System.Collections.Generic;

namespace NoFlo {
    /*
    # This class represents an abstract NoFlo graph containing nodes
    # connected to each other with edges.
    #
    # These graphs can be used for visualization and sketching, but
    # also are the way to start a NoFlo network.
     */
    public class Graph : EventEmitter {

        public string name;
        public Dictionary<string, string> properties;
        public List<Component> nodes;
        public List<InternalSocket> edges;
        // TODO initialisers
        // TODO exports
        public List<InPort> inports;
        public List<OutPort> outports;
        // TODO groups

        /*
name: ''
  caseSensitive: false
  properties: {}
  nodes: []
  edges: []
  initializers: []
  exports: []
  inports: {}
  outports: {}
  groups: []

  # ## Creating new graphs
  #
  # Graphs are created by simply instantiating the Graph class
  # and giving it a name:
  #
  #     myGraph = new Graph 'My very cool graph'
  constructor: (@name = '', options = {}) ->
    @properties = {}
    @nodes = []
    @edges = []
    @initializers = []
    @exports = []
    @inports = {}
    @outports = {}
    @groups = []
    @transaction =
      id: null
      depth: 0

    @caseSensitive = options.caseSensitive or false
    */

        public Graph(string name) {
            this.name = name;

            properties = new Dictionary<string, string>();
            nodes = new List<Component>();
            edges = new List<InternalSocket>();
            inports = new List<InPort>();
            outports = new List<OutPort>();
        }

        /*
  getPortName: (port) ->
    if @caseSensitive then port else port.toLowerCase()
    */

        public void getPortName() {
            throw new NotImplementedException();
        }

        /*
  # ## Group graph changes into transactions
  #
  # If no transaction is explicitly opened, each call to
  # the graph API will implicitly create a transaction for that change
  startTransaction: (id, metadata) ->
    if @transaction.id
      throw Error("Nested transactions not supported")

    @transaction.id = id
    @transaction.depth = 1
    @emit 'startTransaction', id, metadata
    */

        public void startTransaction() {
            throw new NotImplementedException();
        }

        /*
  endTransaction: (id, metadata) ->
    if not @transaction.id
      throw Error("Attempted to end non-existing transaction")

    @transaction.id = null
    @transaction.depth = 0
    @emit 'endTransaction', id, metadata
    */

        public void endTransaction() {
            throw new NotImplementedException();
        }

        /*
  checkTransactionStart: () ->
    if not @transaction.id
      @startTransaction 'implicit'
    else if @transaction.id == 'implicit'
      @transaction.depth += 1
      */

        public void checkTransactionStart() {
            throw new NotImplementedException();
        }

        /*
  checkTransactionEnd: () ->
    if @transaction.id == 'implicit'
      @transaction.depth -= 1
    if @transaction.depth == 0
      @endTransaction 'implicit'
      */

        public void checkTransactionEnd() {
            throw new NotImplementedException();
        }

        /*
  # ## Modifying Graph properties
  #
  # This method allows changing properties of the graph.
  setProperties: (properties) ->
    @checkTransactionStart()
    before = clone @properties
    for item, val of properties
      @properties[item] = val
    @emit 'changeProperties', @properties, before
    @checkTransactionEnd()
    */

        public void setProperties() {
            throw new NotImplementedException();
        }

        /*
  # ## Exporting a port from subgraph
  #
  # This allows subgraphs to expose a cleaner API by having reasonably
  # named ports shown instead of all the free ports of the graph
  #
  # The ports exported using this way are ambiguous in their direciton. Use
  # `addInport` or `addOutport` instead to disambiguate.
  addExport: (publicPort, nodeKey, portKey, metadata = {x:0,y:0}) ->
    platform.deprecated 'noflo.Graph exports is deprecated: please use specific inport or outport instead'
    # Check that node exists
    return unless @getNode nodeKey

    @checkTransactionStart()

    exported =
      public: @getPortName publicPort
      process: nodeKey
      port: @getPortName portKey
      metadata: metadata
    @exports.push exported
    @emit 'addExport', exported

    @checkTransactionEnd()
    */

        public void addExport() {
            throw new NotImplementedException();
        }

        /*
  removeExport: (publicPort) ->
    platform.deprecated 'noflo.Graph exports is deprecated: please use specific inport or outport instead'
    publicPort = @getPortName publicPort
    found = null
    for exported, idx in @exports
      found = exported if exported.public is publicPort

    return unless found
    @checkTransactionStart()
    @exports.splice @exports.indexOf(found), 1
    @emit 'removeExport', found
    @checkTransactionEnd()
    */

        public void removeExport() {
            throw new NotImplementedException();
        }

        /*
  addInport: (publicPort, nodeKey, portKey, metadata) ->
    # Check that node exists
    return unless @getNode nodeKey

    publicPort = @getPortName publicPort
    @checkTransactionStart()
    @inports[publicPort] =
      process: nodeKey
      port: @getPortName portKey
      metadata: metadata
    @emit 'addInport', publicPort, @inports[publicPort]
    @checkTransactionEnd()
    */

        public void addInport() {
            throw new NotImplementedException();
        }

        /*
  removeInport: (publicPort) ->
    publicPort = @getPortName publicPort
    return unless @inports[publicPort]

    @checkTransactionStart()
    port = @inports[publicPort]
    @setInportMetadata publicPort, {}
    delete @inports[publicPort]
    @emit 'removeInport', publicPort, port
    @checkTransactionEnd()
    */

        public void removeInport() {
            throw new NotImplementedException();
        }

        /*
  renameInport: (oldPort, newPort) ->
    oldPort = @getPortName oldPort
    newPort = @getPortName newPort
    return unless @inports[oldPort]

    @checkTransactionStart()
    @inports[newPort] = @inports[oldPort]
    delete @inports[oldPort]
    @emit 'renameInport', oldPort, newPort
    @checkTransactionEnd()
    */

        public void renameInport() {
            throw new NotImplementedException();
        }

        /*
  setInportMetadata: (publicPort, metadata) ->
    publicPort = @getPortName publicPort
    return unless @inports[publicPort]

    @checkTransactionStart()
    before = clone @inports[publicPort].metadata
    @inports[publicPort].metadata = {} unless @inports[publicPort].metadata
    for item, val of metadata
      if val?
        @inports[publicPort].metadata[item] = val
      else
        delete @inports[publicPort].metadata[item]
    @emit 'changeInport', publicPort, @inports[publicPort], before
    @checkTransactionEnd()
    */

        public void setInportMetadata() {
            throw new NotImplementedException();
        }

        /*
  addOutport: (publicPort, nodeKey, portKey, metadata) ->
    # Check that node exists
    return unless @getNode nodeKey

    publicPort = @getPortName publicPort
    @checkTransactionStart()
    @outports[publicPort] =
      process: nodeKey
      port: @getPortName portKey
      metadata: metadata
    @emit 'addOutport', publicPort, @outports[publicPort]

    @checkTransactionEnd()
    */

        public void addOutport() {
            throw new NotImplementedException();
        }

        /*
  removeOutport: (publicPort) ->
    publicPort = @getPortName publicPort
    return unless @outports[publicPort]

    @checkTransactionStart()

    port = @outports[publicPort]
    @setOutportMetadata publicPort, {}
    delete @outports[publicPort]
    @emit 'removeOutport', publicPort, port

    @checkTransactionEnd()
    */

        public void removeOutport() {
            throw new NotImplementedException();
        }

        /*
  renameOutport: (oldPort, newPort) ->
    oldPort = @getPortName oldPort
    newPort = @getPortName newPort
    return unless @outports[oldPort]

    @checkTransactionStart()
    @outports[newPort] = @outports[oldPort]
    delete @outports[oldPort]
    @emit 'renameOutport', oldPort, newPort
    @checkTransactionEnd()
    */

        public void renameOutport() {
            throw new NotImplementedException();
        }

        /*
  setOutportMetadata: (publicPort, metadata) ->
    publicPort = @getPortName publicPort
    return unless @outports[publicPort]

    @checkTransactionStart()
    before = clone @outports[publicPort].metadata
    @outports[publicPort].metadata = {} unless @outports[publicPort].metadata
    for item, val of metadata
      if val?
        @outports[publicPort].metadata[item] = val
      else
        delete @outports[publicPort].metadata[item]
    @emit 'changeOutport', publicPort, @outports[publicPort], before
    @checkTransactionEnd()
    */

        public void setOutportMetadata() {
            throw new NotImplementedException();
        }

        /*
  # ## Grouping nodes in a graph
  #
  addGroup: (group, nodes, metadata) ->
    @checkTransactionStart()

    g =
      name: group
      nodes: nodes
      metadata: metadata
    @groups.push g
    @emit 'addGroup', g

    @checkTransactionEnd()
    */

        public void addGroup() {
            throw new NotImplementedException();
        }

        /*
  renameGroup: (oldName, newName) ->
    @checkTransactionStart()
    for group in @groups
      continue unless group
      continue unless group.name is oldName
      group.name = newName
      @emit 'renameGroup', oldName, newName
    @checkTransactionEnd()
    */

        public void renameGroup() {
            throw new NotImplementedException();
        }

        /*
  removeGroup: (groupName) ->
    @checkTransactionStart()

    for group in @groups
      continue unless group
      continue unless group.name is groupName
      @setGroupMetadata group.name, {}
      @groups.splice @groups.indexOf(group), 1
      @emit 'removeGroup', group

    @checkTransactionEnd()
    */

        public void removeGroup() {
            throw new NotImplementedException();
        }

        /*
  setGroupMetadata: (groupName, metadata) ->
    @checkTransactionStart()
    for group in @groups
      continue unless group
      continue unless group.name is groupName
      before = clone group.metadata
      for item, val of metadata
        if val?
          group.metadata[item] = val
        else
          delete group.metadata[item]
      @emit 'changeGroup', group, before
    @checkTransactionEnd()
    */

        public void setGroupMetadata() {
            throw new NotImplementedException();
        }

        /*
  # ## Adding a node to the graph
  #
  # Nodes are identified by an ID unique to the graph. Additionally,
  # a node may contain information on what NoFlo component it is and
  # possible display coordinates.
  #
  # For example:
  #
  #     myGraph.addNode 'Read, 'ReadFile',
  #       x: 91
  #       y: 154
  #
  # Addition of a node will emit the `addNode` event.
  addNode: (id, component, metadata) ->
    @checkTransactionStart()

    metadata = {} unless metadata
    node =
      id: id
      component: component
      metadata: metadata
    @nodes.push node
    @emit 'addNode', node

    @checkTransactionEnd()
    node
    */

        public void addNode() {
            throw new NotImplementedException();
        }

        /*
  # ## Removing a node from the graph
  #
  # Existing nodes can be removed from a graph by their ID. This
  # will remove the node and also remove all edges connected to it.
  #
  #     myGraph.removeNode 'Read'
  #
  # Once the node has been removed, the `removeNode` event will be
  # emitted.
  removeNode: (id) ->
    node = @getNode id
    return unless node

    @checkTransactionStart()

    toRemove = []
    for edge in @edges
      if (edge.from.node is node.id) or (edge.to.node is node.id)
        toRemove.push edge
    for edge in toRemove
      @removeEdge edge.from.node, edge.from.port, edge.to.node, edge.to.port

    toRemove = []
    for initializer in @initializers
      if initializer.to.node is node.id
        toRemove.push initializer
    for initializer in toRemove
      @removeInitial initializer.to.node, initializer.to.port

    toRemove = []
    for exported in @exports
      if @getPortName(id) is exported.process
        toRemove.push exported
    for exported in toRemove
      @removeExport exported.public

    toRemove = []
    for pub, priv of @inports
      if priv.process is id
        toRemove.push pub
    for pub in toRemove
      @removeInport pub

    toRemove = []
    for pub, priv of @outports
      if priv.process is id
        toRemove.push pub
    for pub in toRemove
      @removeOutport pub

    for group in @groups
      continue unless group
      index = group.nodes.indexOf(id)
      continue if index is -1
      group.nodes.splice index, 1

    @setNodeMetadata id, {}

    if -1 isnt @nodes.indexOf node
      @nodes.splice @nodes.indexOf(node), 1

    @emit 'removeNode', node

    @checkTransactionEnd()
    */

        public void removeNode() {
            throw new NotImplementedException();
        }

        /*
  # ## Getting a node
  #
  # Nodes objects can be retrieved from the graph by their ID:
  #
  #     myNode = myGraph.getNode 'Read'
  getNode: (id) ->
    for node in @nodes
      continue unless node
      return node if node.id is id
    return null
    */

        public void getNode() {
            throw new NotImplementedException();
        }

        /*
  # ## Renaming a node
  #
  # Nodes IDs can be changed by calling this method.
  renameNode: (oldId, newId) ->
    @checkTransactionStart()

    node = @getNode oldId
    return unless node
    node.id = newId

    for edge in @edges
      continue unless edge
      if edge.from.node is oldId
        edge.from.node = newId
      if edge.to.node is oldId
        edge.to.node = newId

    for iip in @initializers
      continue unless iip
      if iip.to.node is oldId
        iip.to.node = newId

    for pub, priv of @inports
      if priv.process is oldId
        priv.process = newId
    for pub, priv of @outports
      if priv.process is oldId
        priv.process = newId
    for exported in @exports
      if exported.process is oldId
        exported.process = newId

    for group in @groups
      continue unless group
      index = group.nodes.indexOf(oldId)
      continue if index is -1
      group.nodes[index] = newId

    @emit 'renameNode', oldId, newId
    @checkTransactionEnd()
    */

        public void renameNode() {
            throw new NotImplementedException();
        }

        /*
  # ## Changing a node's metadata
  #
  # Node metadata can be set or changed by calling this method.
  setNodeMetadata: (id, metadata) ->
    node = @getNode id
    return unless node

    @checkTransactionStart()

    before = clone node.metadata
    node.metadata = {} unless node.metadata

    for item, val of metadata
      if val?
        node.metadata[item] = val
      else
        delete node.metadata[item]

    @emit 'changeNode', node, before
    @checkTransactionEnd()
    */

        public void setNodeMetadata() {
            throw new NotImplementedException();
        }

        /*
  # ## Connecting nodes
  #
  # Nodes can be connected by adding edges between a node's outport
  # and another node's inport:
  #
  #     myGraph.addEdge 'Read', 'out', 'Display', 'in'
  #     myGraph.addEdgeIndex 'Read', 'out', null, 'Display', 'in', 2
  #
  # Adding an edge will emit the `addEdge` event.
  addEdge: (outNode, outPort, inNode, inPort, metadata = {}) ->
    outPort = @getPortName outPort
    inPort = @getPortName inPort
    for edge in @edges
      # don't add a duplicate edge
      return if (edge.from.node is outNode and edge.from.port is outPort and edge.to.node is inNode and edge.to.port is inPort)
    return unless @getNode outNode
    return unless @getNode inNode

    @checkTransactionStart()

    edge =
      from:
        node: outNode
        port: outPort
      to:
        node: inNode
        port: inPort
      metadata: metadata
    @edges.push edge
    @emit 'addEdge', edge

    @checkTransactionEnd()
    edge
    */

        public void addEdge() {
            throw new NotImplementedException();
        }

        /*
  # Adding an edge will emit the `addEdge` event.
  addEdgeIndex: (outNode, outPort, outIndex, inNode, inPort, inIndex, metadata = {}) ->
    return unless @getNode outNode
    return unless @getNode inNode

    outPort = @getPortName outPort
    inPort = @getPortName inPort

    inIndex = undefined if inIndex is null
    outIndex = undefined if outIndex is null
    metadata = {} unless metadata

    @checkTransactionStart()

    edge =
      from:
        node: outNode
        port: outPort
        index: outIndex
      to:
        node: inNode
        port: inPort
        index: inIndex
      metadata: metadata
    @edges.push edge
    @emit 'addEdge', edge

    @checkTransactionEnd()
    edge
    */

        public void addEdgeIndex() {
            throw new NotImplementedException();
        }

        /*
  # ## Disconnected nodes
  #
  # Connections between nodes can be removed by providing the
  # nodes and ports to disconnect.
  #
  #     myGraph.removeEdge 'Display', 'out', 'Foo', 'in'
  #
  # Removing a connection will emit the `removeEdge` event.
  removeEdge: (node, port, node2, port2) ->
    @checkTransactionStart()
    port = @getPortName port
    port2 = @getPortName port2
    toRemove = []
    toKeep = []
    if node2 and port2
      for edge,index in @edges
        if edge.from.node is node and edge.from.port is port and edge.to.node is node2 and edge.to.port is port2
          @setEdgeMetadata edge.from.node, edge.from.port, edge.to.node, edge.to.port, {}
          toRemove.push edge
        else
          toKeep.push edge
    else
      for edge,index in @edges
        if (edge.from.node is node and edge.from.port is port) or (edge.to.node is node and edge.to.port is port)
          @setEdgeMetadata edge.from.node, edge.from.port, edge.to.node, edge.to.port, {}
          toRemove.push edge
        else
          toKeep.push edge

    @edges = toKeep
    for edge in toRemove
      @emit 'removeEdge', edge

    @checkTransactionEnd()
    */
        
        public void removeEdge() {
            throw new NotImplementedException();
        }

        /*
  # ## Getting an edge
  #
  # Edge objects can be retrieved from the graph by the node and port IDs:
  #
  #     myEdge = myGraph.getEdge 'Read', 'out', 'Write', 'in'
  getEdge: (node, port, node2, port2) ->
    port = @getPortName port
    port2 = @getPortName port2
    for edge,index in @edges
      continue unless edge
      if edge.from.node is node and edge.from.port is port
        if edge.to.node is node2 and edge.to.port is port2
          return edge
    return null
    */

        public void getEdge() {
            throw new NotImplementedException();
        }

        /*
  # ## Changing an edge's metadata
  #
  # Edge metadata can be set or changed by calling this method.
  setEdgeMetadata: (node, port, node2, port2, metadata) ->
    edge = @getEdge node, port, node2, port2
    return unless edge

    @checkTransactionStart()
    before = clone edge.metadata
    edge.metadata = {} unless edge.metadata

    for item, val of metadata
      if val?
        edge.metadata[item] = val
      else
        delete edge.metadata[item]

    @emit 'changeEdge', edge, before
    @checkTransactionEnd()
    */

        public void setEdgeMetadata() {
            throw new NotImplementedException();
        }

        /*
  # ## Adding Initial Information Packets
  #
  # Initial Information Packets (IIPs) can be used for sending data
  # to specified node inports without a sending node instance.
  #
  # IIPs are especially useful for sending configuration information
  # to components at NoFlo network start-up time. This could include
  # filenames to read, or network ports to listen to.
  #
  #     myGraph.addInitial 'somefile.txt', 'Read', 'source'
  #     myGraph.addInitialIndex 'somefile.txt', 'Read', 'source', 2
  #
  # If inports are defined on the graph, IIPs can be applied calling
  # the `addGraphInitial` or `addGraphInitialIndex` methods.
  #
  #     myGraph.addGraphInitial 'somefile.txt', 'file'
  #     myGraph.addGraphInitialIndex 'somefile.txt', 'file', 2
  #
  # Adding an IIP will emit a `addInitial` event.
  addInitial: (data, node, port, metadata) ->
    return unless @getNode node

    port = @getPortName port
    @checkTransactionStart()
    initializer =
      from:
        data: data
      to:
        node: node
        port: port
      metadata: metadata
    @initializers.push initializer
    @emit 'addInitial', initializer

    @checkTransactionEnd()
    initializer
    */

        public void addInitial() {
            throw new NotImplementedException();
        }

        /*
  addInitialIndex: (data, node, port, index, metadata) ->
    return unless @getNode node
    index = undefined if index is null

    port = @getPortName port
    @checkTransactionStart()
    initializer =
      from:
        data: data
      to:
        node: node
        port: port
        index: index
      metadata: metadata
    @initializers.push initializer
    @emit 'addInitial', initializer

    @checkTransactionEnd()
    initializer
    */

        public void addInitialIndex() {
            throw new NotImplementedException();
        }

        /*
  addGraphInitial: (data, node, metadata) ->
    inport = @inports[node]
    return unless inport
    @addInitial data, inport.process, inport.port, metadata
    */

        public void addGraphInitial() {
            throw new NotImplementedException();
        }

        /*
  addGraphInitialIndex: (data, node, index, metadata) ->
    inport = @inports[node]
    return unless inport
    @addInitialIndex data, inport.process, inport.port, index, metadata
    */

        public void addGraphInitialIndex() {
            throw new NotImplementedException();
        }

        /*
  # ## Removing Initial Information Packets
  #
  # IIPs can be removed by calling the `removeInitial` method.
  #
  #     myGraph.removeInitial 'Read', 'source'
  #
  # If the IIP was applied via the `addGraphInitial` or
  # `addGraphInitialIndex` functions, it can be removed using
  # the `removeGraphInitial` method.
  #
  #     myGraph.removeGraphInitial 'file'
  #
  # Remove an IIP will emit a `removeInitial` event.
  removeInitial: (node, port) ->
    port = @getPortName port
    @checkTransactionStart()

    toRemove = []
    toKeep = []
    for edge, index in @initializers
      if edge.to.node is node and edge.to.port is port
        toRemove.push edge
      else
        toKeep.push edge
    @initializers = toKeep
    for edge in toRemove
      @emit 'removeInitial', edge

    @checkTransactionEnd()
    */

        public void removeInitial() {
            throw new NotImplementedException();
        }

        /*
  removeGraphInitial: (node) ->
    inport = @inports[node]
    return unless inport
    @removeInitial inport.process, inport.port
    */

        public void removeGraphInitial() {
            throw new NotImplementedException();
        }

        /*
  toDOT: ->
    cleanID = (id) ->
      id.replace /\s* /   // Note that this was change because of c# comments
        g, ""
    cleanPort = (port) ->
      port.replace /\./g, ""

    dot = "digraph {\n"

    for node in @nodes
      dot += "    #{cleanID(node.id)} [label=#{node.id} shape=box]\n"

    for initializer, id in @initializers
      if typeof initializer.from.data is 'function'
        data = 'Function'
      else
        data = initializer.from.data
      dot += "    data#{id} [label=\"'#{data}'\" shape=plaintext]\n"
      dot += "    data#{id} -> #{cleanID(initializer.to.node)}[headlabel=#{cleanPort(initializer.to.port)} labelfontcolor=blue labelfontsize=8.0]\n"

    for edge in @edges
      dot += "    #{cleanID(edge.from.node)} -> #{cleanID(edge.to.node)}[taillabel=#{cleanPort(edge.from.port)} headlabel=#{cleanPort(edge.to.port)} labelfontcolor=blue labelfontsize=8.0]\n"

    dot += "}"

    return dot
    */

        public void toDOT() {
            throw new NotImplementedException();
        }

        /*
  toYUML: ->
    yuml = []

    for initializer in @initializers
      yuml.push "(start)[#{initializer.to.port}]->(#{initializer.to.node})"

    for edge in @edges
      yuml.push "(#{edge.from.node})[#{edge.from.port}]->(#{edge.to.node})"
    yuml.join ","
    */

        public void toYUML() {
            throw new NotImplementedException();
        }

        /*
  toJSON: ->
    json =
      caseSensitive: @caseSensitive
      properties: { }
        inports: {}
      outports: {}
      groups: []
        processes: {}
      connections: []

        json.properties.name = @name if @name
    for property, value of @properties
      json.properties[property] = value

    for pub, priv of @inports
      json.inports[pub] = priv
    for pub, priv of @outports
      json.outports[pub] = priv

    # Legacy exported ports
    for exported in @exports
      json.exports = [] unless json.exports
      json.exports.push exported

    for group in @groups
      groupData =
        name: group.name
        nodes: group.nodes
      if Object.keys(group.metadata).length
        groupData.metadata = group.metadata
      json.groups.push groupData

    for node in @nodes
      json.processes[node.id] =
        component: node.component
      if node.metadata
        json.processes[node.id].metadata = node.metadata

    for edge in @edges
      connection =
        src:
          process: edge.from.node
          port: edge.from.port
          index: edge.from.index
        tgt:
          process: edge.to.node
          port: edge.to.port
          index: edge.to.index
      connection.metadata = edge.metadata if Object.keys(edge.metadata).length
      json.connections.push connection

    for initializer in @initializers
      json.connections.push
        data: initializer.from.data
        tgt:
          process: initializer.to.node
          port: initializer.to.port
          index: initializer.to.index

    json
    */

        public void toJSON() {
            throw new NotImplementedException();
        }

        /*
  save: (file, callback) ->
    if platform.isBrowser()
      return callback new Error "Saving graphs not supported on browser"

    json = JSON.stringify @toJSON(), null, 4
    require('fs').writeFile "#{file}.json", json, "utf-8", (err, data) ->
      throw err if err
      callback file
      */

        public void save() {
            throw new NotImplementedException();
        }


        // TODO
        /*
exports.Graph = Graph

exports.createGraph = (name, options) ->
  new Graph name, options

exports.loadJSON = (definition, callback, metadata = {}) ->
  definition = JSON.parse definition if typeof definition is 'string'
  definition.properties = {} unless definition.properties
  definition.processes = { }
        unless definition.processes

definition.connections = [] unless definition.connections
caseSensitive = definition.caseSensitive or false


graph = new Graph definition.properties.name, { caseSensitive}

        graph.startTransaction 'loadJSON', metadata
        properties = { }
  for property, value of definition.properties
    continue if property is 'name'
    properties[property] = value
  graph.setProperties properties

  for id, def of definition.processes
    def.metadata = { }
        unless def.metadata

graph.addNode id, def.component, def.metadata

  for conn in definition.connections
metadata = if conn.metadata then conn.metadata else { }
    if conn.data isnt undefined
      if typeof conn.tgt.index is 'number'
        graph.addInitialIndex conn.data, conn.tgt.process, graph.getPortName(conn.tgt.port), conn.tgt.index, metadata
      else
        graph.addInitial conn.data, conn.tgt.process, graph.getPortName(conn.tgt.port), metadata
      continue
    if typeof conn.src.index is 'number' or typeof conn.tgt.index is 'number'
      graph.addEdgeIndex conn.src.process, graph.getPortName(conn.src.port), conn.src.index, conn.tgt.process, graph.getPortName(conn.tgt.port), conn.tgt.index, metadata
      continue
    graph.addEdge conn.src.process, graph.getPortName(conn.src.port), conn.tgt.process, graph.getPortName(conn.tgt.port), metadata

  if definition.exports and definition.exports.length
    for exported in definition.exports
      if exported.private
# Translate legacy ports to new
        split = exported.private.split('.')
        continue unless split.length is 2
        processId = split[0]
        portId = split[1]

        # Get properly cased process id
        for id of definition.processes
          if graph.getPortName(id) is graph.getPortName(processId)
            processId = id
      else
        processId = exported.process
        portId = graph.getPortName exported.port
      graph.addExport exported.public, processId, portId, exported.metadata

  if definition.inports
    for pub, priv of definition.inports
      graph.addInport pub, priv.process, graph.getPortName(priv.port), priv.metadata
  if definition.outports
    for pub, priv of definition.outports
      graph.addOutport pub, priv.process, graph.getPortName(priv.port), priv.metadata

  if definition.groups
    for group in definition.groups
      graph.addGroup group.name, group.nodes, group.metadata || { }

        graph.endTransaction 'loadJSON'

  callback null, graph

exports.loadFBP = (fbpData, callback, metadata = { }, caseSensitive = false) ->
  try
    definition = require('fbp').parse fbpData, {caseSensitive
    }
  catch e
    return callback e
  exports.loadJSON definition, callback, metadata

exports.loadHTTP = (url, callback) ->
  req = new XMLHttpRequest
  req.onreadystatechange = ->
    return unless req.readyState is 4
    unless req.status is 200
      return callback new Error "Failed to load #{url}: HTTP #{req.status}"
    callback null, req.responseText
  req.open 'GET', url, true
  req.send()

exports.loadFile = (file, callback, metadata = {}, caseSensitive = false) ->
  if platform.isBrowser()
    # On browser we can try getting the file via AJAX
    exports.loadHTTP file, (err, data) ->
      return callback err if err
      if file.split('.').pop() is 'fbp'
        return exports.loadFBP data, callback, metadata
      definition = JSON.parse data
      exports.loadJSON definition, callback, metadata
    return
  # Node.js graph file
  require('fs').readFile file, "utf-8", (err, data) ->
    return callback err if err

    if file.split('.').pop() is 'fbp'
      return exports.loadFBP data, callback, {}, caseSensitive

    definition = JSON.parse data
    exports.loadJSON definition, callback, { }

# remove everything in the graph
resetGraph = (graph) ->

  # Edges and similar first, to have control over the order
  # If we'd do nodes first, it will implicitly delete edges
  # Important to make journal transactions invertible
  for group in (clone graph.groups).reverse()
    graph.removeGroup group.name if group?
  for port, v of clone graph.outports
    graph.removeOutport port
  for port, v of clone graph.inports
    graph.removeInport port
  for exp in clone (graph.exports).reverse()
    graph.removeExport exp.public
# XXX: does this actually null the props??
  graph.setProperties {}
  for iip in (clone graph.initializers).reverse()
    graph.removeInitial iip.to.node, iip.to.port
  for edge in (clone graph.edges).reverse()
    graph.removeEdge edge.from.node, edge.from.port, edge.to.node, edge.to.port
  for node in (clone graph.nodes).reverse()
    graph.removeNode node.id

# Note: Caller should create transaction
# First removes everything in @base, before building it up to mirror @to
mergeResolveTheirsNaive = (base, to) ->
  resetGraph base

  for node in to.nodes
    base.addNode node.id, node.component, node.metadata
  for edge in to.edges
    base.addEdge edge.from.node, edge.from.port, edge.to.node, edge.to.port, edge.metadata
  for iip in to.initializers
    base.addInitial iip.from.data, iip.to.node, iip.to.port, iip.metadata
  for exp in to.exports
    base.addExport exp.public, exp.node, exp.port, exp.metadata
  base.setProperties to.properties
  for pub, priv of to.inports
    base.addInport pub, priv.process, priv.port, priv.metadata
  for pub, priv of to.outports
    base.addOutport pub, priv.process, priv.port, priv.metadata
  for group in to.groups
    base.addGroup group.name, group.nodes, group.metadata

exports.equivalent = (a, b, options = { }
) ->
  # TODO: add option to only compare known fields
  # TODO: add option to ignore metadata
  A = JSON.stringify a
  B = JSON.stringify b
  return A == B
         */
    }

}