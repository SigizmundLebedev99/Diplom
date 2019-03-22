const state = ()=>({
    dialog:false,
    predefinedCode:null,
});

const getters = {
    dialog:(state)=>state.dialog
}

const actions = {
    preWICreating({commit}, predefinedCode){
        commit('setDialog', true);
    },
    setDialog({commit}, dialog){
        commit('setDialog', dialog);
        if(!dialog){
            commit('clear');
        }
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