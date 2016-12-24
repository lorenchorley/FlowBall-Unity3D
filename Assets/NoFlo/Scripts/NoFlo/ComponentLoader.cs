
using NoFloEditor;
using System;
using System.Collections.Generic;

namespace NoFlo { 

    public class ComponentLoader : EventEmitter {

        List<Component> components;
        Dictionary<string, object> libraryIcons;
        bool processing;
        bool ready;

        /*
constructor: (@baseDir, @options = {}) ->
    @components = null
    @libraryIcons = {}
    @processing = false
    @ready = false
    @setMaxListeners 0 if typeof @setMaxListeners is 'function'
    */

        public ComponentLoader() {
            InitialiseEvent("ready");
            FinishInitialisation();

            components = new List<Component>();
            libraryIcons = new Dictionary<string, object>();
            processing = false;
            ready = false;
        }

        /*
  getModulePrefix: (name) ->
    return '' unless name
    return '' if name is 'noflo'
    name = name.replace /\@[a-z\-]+\//, '' if name[0] is '@'
    name.replace 'noflo-', ''
    */

        public string getModulePrefix(string name) {
            return name; // TODO
        }

        /*
  listComponents: (callback) ->
    if @processing
      @once 'ready', =>
        callback null, @components
      return
    return callback null, @components if @components

    @ready = false
    @processing = true

    @components = {}
    registerLoader.register @, (err) =>
      if err
        return callback err if callback
        throw err
      @processing = false
      @ready = true
      @emit 'ready', true
      callback null, @components if callback
      */

        public void listComponents(Action callback) {
            throw new NotImplementedException();
        }

        /*
  load: (name, callback, metadata) ->
    unless @ready
      @listComponents (err) =>
        return callback err if err
        @load name, callback, metadata
      return

    component = @components[name]
    unless component
      # Try an alias
      for componentName of @components
        if componentName.split('/')[1] is name
          component = @components[componentName]
          break
      unless component
        # Failure to load
        callback new Error "Component #{name} not available with base #{@baseDir}"
        return

    if @isGraph component
      if typeof process isnt 'undefined' and process.execPath and process.execPath.indexOf('node') isnt -1
        # nextTick is faster on Node.js
        process.nextTick =>
          @loadGraph name, component, callback, metadata
      else
        setTimeout =>
          @loadGraph name, component, callback, metadata
        , 0
      return

    @createComponent name, component, metadata, (err, instance) =>
      return callback err if err
      if not instance
        callback new Error "Component #{name} could not be loaded."
        return

      instance.baseDir = @baseDir if name is 'Graph'
      @setIcon name, instance
      callback null, instance
      */

        public void load(string name, Action callback, Dictionary<string, string> metadata) {
            throw new NotImplementedException();
        }

        /*
  # Creates an instance of a component.
  createComponent: (name, component, metadata, callback) ->
    implementation = component
    unless implementation
      return callback new Error "Component #{name} not available"

    # If a string was specified, attempt to `require` it.
    if typeof implementation is 'string'
      if typeof registerLoader.dynamicLoad is 'function'
        registerLoader.dynamicLoad name, implementation, metadata, callback
        return
      return callback Error "Dynamic loading of #{implementation} for component #{name} not available on this platform."

    # Attempt to create the component instance using the `getComponent` method.
    if typeof implementation.getComponent is 'function'
      instance = implementation.getComponent metadata
    # Attempt to create a component using a factory function.
    else if typeof implementation is 'function'
      instance = implementation metadata
    else
      callback new Error "Invalid type #{typeof(implementation)} for component #{name}."
      return

    instance.componentName = name if typeof name is 'string'
    callback null, instance
    */

        public Component createComponent(string name, string component) {
            Type componentType;
            if (!ComponentCatalog.RequestComponentsByQualifiedName().TryGetValue(name, out componentType))
                throw new Exception("TODO");

            Component instance = (Component) Activator.CreateInstance(componentType);
            instance.componentName = name;

            return instance;
        }

        /*
  isGraph: (cPath) ->
    # Live graph instance
    return true if typeof cPath is 'object' and cPath instanceof nofloGraph.Graph
    # Graph JSON definition
    return true if typeof cPath is 'object' and cPath.processes and cPath.connections
    return false unless typeof cPath is 'string'
    # Graph file path
    cPath.indexOf('.fbp') isnt -1 or cPath.indexOf('.json') isnt -1
    */

        public void isGraph() {
            throw new NotImplementedException();
        }

        /*
  loadGraph: (name, component, callback, metadata) ->
    @createComponent name, @components['Graph'], metadata, (err, graph) =>
      return callback err if err
      graphSocket = internalSocket.createSocket()
      graph.loader = @
      graph.baseDir = @baseDir
      graph.inPorts.graph.attach graphSocket
      graphSocket.send component
      graphSocket.disconnect()
      graph.inPorts.remove 'graph'
      @setIcon name, graph
      callback null, graph
      */

        public void loadGraph(string name, string component, Action callback, Dictionary<string, string> metadata) {
            Graph graph = new Graph(name);
            InternalSocket graphSocket = new InternalSocket();
            throw new NotImplementedException();
        }

        /*
  setIcon: (name, instance) ->
    # See if component has an icon
    return if not instance.getIcon or instance.getIcon()

    # See if library has an icon
    [library, componentName] = name.split '/'
    if componentName and @getLibraryIcon library
      instance.setIcon @getLibraryIcon library
      return

    # See if instance is a subgraph
    if instance.isSubgraph()
      instance.setIcon 'sitemap'
      return

    instance.setIcon 'square'
    return
    */

        public void setIcon() {
            throw new NotImplementedException();
        }

        /*
  getLibraryIcon: (prefix) ->
    if @libraryIcons[prefix]
      return @libraryIcons[prefix]
    return null
    */

        public void getLibraryIcon() {
            throw new NotImplementedException();
        }

        /*
  setLibraryIcon: (prefix, icon) ->
    @libraryIcons[prefix] = icon
    */

        public void setLibraryIcon() {
            throw new NotImplementedException();
        }

        /*
  normalizeName: (packageId, name) ->
    prefix = @getModulePrefix packageId
    fullName = "#{prefix}/#{name}"
    fullName = name unless packageId
    fullName
    */

        public void normalizeName() {
            throw new NotImplementedException();
        }

        /*
  registerComponent: (packageId, name, cPath, callback) ->
    fullName = @normalizeName packageId, name
    @components[fullName] = cPath
    do callback if callback
    */

        public void registerComponent() {
            throw new NotImplementedException();
        }

        /*
  registerGraph: (packageId, name, gPath, callback) ->
    @registerComponent packageId, name, gPath, callback
    */

        public void registerGraph() {
            throw new NotImplementedException();
        }

        /*
  registerLoader: (loader, callback) ->
    loader @, callback
    */

        public void registerLoader() {
            throw new NotImplementedException();
        }

        /*
  setSource: (packageId, name, source, language, callback) ->
    unless registerLoader.setSource
      return callback new Error 'setSource not allowed'

    unless @ready
      @listComponents (err) =>
        return callback err if err
        @setSource packageId, name, source, language, callback
      return

    registerLoader.setSource @, packageId, name, source, language, callback
    */

        public void setSource() {
            throw new NotImplementedException();
        }

        /*
  getSource: (name, callback) ->
    unless registerLoader.getSource
      return callback new Error 'getSource not allowed'
    unless @ready
      @listComponents (err) =>
        return callback err if err
        @getSource name, callback
      return

    registerLoader.getSource @, name, callback
    */

        public void getSource() {
            throw new NotImplementedException();
        }

        /*
  clear: ->
    @components = null
    @ready = false
    @processing = false
         */

        public void clear() {
            throw new NotImplementedException();
        }

    }

}
 