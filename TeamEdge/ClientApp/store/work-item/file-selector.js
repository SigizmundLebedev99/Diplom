import axios from 'axios'

const getters = {
    selectedFiles:(state)=>state.selectedFiles,
    files:(state)=>state.files.filter(e=>state.selectedFiles.indexOf(e)<0),
    opened:(state)=>state.opened,
    loading:(state)=>state.loading
};

const actions = {
    open({commit, dispatch}, payload){
        dispatch('fetchFiles');
        commit('setOpened', true);
        if(payload)
            commit('setSelected',payload);
    },
    fetchFiles({commit, rootGetters}){
        commit('setLoading');
        axios.get(`/api/project/${rootGetters['project/project'].id}/files`)
        .then(
            r=>{
                commit('setFiles',r.data); 
                commit('setLoading');
            },
            r=>{console.log(r.response)})
    },
    uploadFile({commit, rootGetters}, file){
        const data = new FormData();
        data.append( 'file', file );
        axios.post(`/api/file/project/${rootGetters['project/project'].id}`,data,
        { headers: {'Content-Type': 'multipart/form-data' }})
        .then(
            r=>{commit('addFile', r.data)},
            r=>console.log(`Couldn't upload file: ${ file.name }.`)
        );
    }
}

const mutations = {
    setFiles(state, payload){
        state.files = payload;
    },
    setSelectedFiles(state, payload){
        state.selectedFiles = payload;
    },
    setOpened(state, payload){
        state.opened = payload;
    },
    addFile(state, payload){
        state.selectedFiles.push(payload);        
    },
    removeFile(state, payload){
        state.selectedFiles.splice(state.selectedFiles.indexOf(state.selectedFiles.find(e=>e.id = payload)),1);
    },
    setLoading(state){
        state.loading = !state.loading;
    },
    clear(state){
        state.files = [];
        state.selectedFiles = [];
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