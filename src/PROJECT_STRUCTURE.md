/app
    //STATE SHOULD BE A FIRST LEVEL LAYER TOO, BECAUSE
    //MANY MODULES CAN BE DEPENDENCE ON A SUBSTATE
    //BUT A SUBSTATE CAN ONLY HAVE ONE REDUCER: https://github.com/ngrx/platform/issues/1629
    /state
        /substate
            substate.actions.ts
            substate.reducers.ts
            substate.selectors.ts
            substate.effects.ts
            substateState.interface.ts (interface of shape of the substate)
    /shared
        shared.module.ts        
        /services
        /components
    /core    
        core.module.ts
        /layout
        /ui-models
        /models
        /directives
        /guards
        /interceptors
        /services (can also be put in the shared)
    /feature-A
        feature-A.routes.ts
        feature-A.module.ts
        /types (or api or api models)
        /services (OPTIONAL: if the service is provided when lazy load this module)
        /components       
                    
    