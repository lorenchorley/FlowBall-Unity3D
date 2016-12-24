
using System;

namespace NoFlo { 

    public class Ports {
        /*
         model: InPort
  constructor: (ports) ->
    @ports = {}
    return unless ports
    for name, options of ports
      @add name, options
      */

        public Ports() {
            throw new NotImplementedException();
        }

        /*
  add: (name, options, process) ->
    if name is 'add' or name is 'remove'
      throw new Error 'Add and remove are restricted port names'

    unless name.match /^[a-z0-9_\.\/]+$/
      throw new Error "Port names can only contain lowercase alphanumeric characters and underscores. '#{name}' not allowed"

    # Remove previous implementation
    @remove name if @ports[name]

    if typeof options is 'object' and options.canAttach
      @ports[name] = options
    else
      @ports[name] = new @model options, process

    @[name] = @ports[name]

    @emit 'add', name

    @ # chainable
    */

        public void add() {
            throw new NotImplementedException();
        }

        /*
  remove: (name) ->
    throw new Error "Port #{name} not defined" unless @ports[name]
    delete @ports[name]
    delete @[name]
    @emit 'remove', name

    @ # chainable
         */

        public void remove() {
            throw new NotImplementedException();
        }

    }

}
 