const state = ()=>({
    dialog:false,
    predefined:null,
    additionalInfo:{}
});

const getters = {
    dialog:(state)=>state.dialog,
    additionalInfo:(state)=>state.additionalInfo,
    predefined:state=>state.predefined
}

const actions = {
    preWICreating({commit}, predefined){
        commit('predefined', predefined);
        commit('setDialog', true);
    },
    setDialog({commit}, dialog){
        commit('setDialog', dialog);
        commit('fileSelector/clear',null,{root:true});
    }
}

const mutations = {
    setDialog(state, dialog){
        state.dialog = dialog;
    },
    predefined(state,payload){
        state.predefined = payload;
        if(payload && payload.parent){
            state.additionalInfo.parentId = payload.parent.descriptionId;
        }
    },
    setAdditionalInfo(state, payload){
        state.additionalInfo = payload;
    }
}

export default{
    namespaced:true,
    state,
    getters,
    mutations,
    actions
}