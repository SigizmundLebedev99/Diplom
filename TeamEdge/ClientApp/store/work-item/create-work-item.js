const state = ()=>({
    dialog:false,
    predefinedCode:null,
});

const getters = {
    dialog:(state)=>state.dialog
}

const actions = {
    preWICreating({commit}, predefinedCode){
        commit('predefined', predefinedCode);
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
        state.predefinedCode = payload;
    }
}

export default{
    namespaced:true,
    state,
    getters,
    mutations,
    actions
}