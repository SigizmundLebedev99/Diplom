import axios from 'axios'

const getters = {
    selectedFiles:(state)=>state.selectedFiles,
    files:(state)=>state.files,
    opened:(state)=>state.opened,
    loading:(state)=>state.loading
};

const actions = {
    open({commit, dispatch}){
        dispatch('fetchFiles');
        commit('setOpened', true);
    },
    fetchFiles({commit, state, rootGetters}){
        commit('setLoading');
        axios.get(`api/project/${rootGetters['project/project'].id}/files`)
        .then(
            r=>{
                //.filter(e=>state.selectedFiles.map(f=>f.id).indexOf(e.id)<0)
                commit('setFiles',r.data); 
                commit('setLoading');
            },
            r=>{console.log(r.response)})
    }
}

const mutations = {
    setFiles(state, payload){
        if(payload)
            state.files = payload;
        else
            state.files = null;
    },
    setOpened(state, payload){
        state.opened = payload;
    },
    addFile(state, payload){
        state.files.push(payload);        
    },
    setLoading(state){
        state.loading = !state.loading;
    }
};

const state = ()=>({
    selectedFiles:[],
    files:[],
    opened:false,
    loading:false
});

export default {
    namespaced: true,
    mutations,
    getters,
    actions,
    state
}