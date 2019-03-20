const state = ()=>({
    dialog:false,
    predefinedCode:null,
    fileIds:[],
    tags:[]
});

const getters = {
    dialog:(state)=>state.dialog
}

const actions = {
    preWICreating({commit, rootState}, predefinedCode){
        commit('setDialog', {dialog:true, predefinedCode});
        commit('setProject', rootState.getters['project/project'].id);
    }
}

const mutations = {
    setDialog(state, {dialog,predefinedCode}){
        state.dialog = dialog;
        if(!dialog)
            state.predefinedCode = null;
        else
        {
            if(predefinedCode)
                state.predefinedCode = predefinedCode;
        }
    },
    setProject(state, payload){
        state.entity.projectId = payload;
    },
    addFileId(state, payload){
        if(state.dialog)
            state.fileIds.push(payload);
    },
    clear(state){
        if(state.fileIds)
            state.fileIds = [];
        if(state.tags)
            state.tags = [];
    }
}

export default{
    namespaced:true,
    state,
    getters,
    mutations,
    actions
}