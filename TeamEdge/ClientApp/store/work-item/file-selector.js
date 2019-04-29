import axios from 'axios'

const state = ()=>({
    selectedFiles:[],
    files:[],
    opened:false,
    loading:false,
    onClose:null,
});

const getters = {
    selectedFiles:(state)=>state.selectedFiles,
    files:(state)=>state.files.filter(e=>state.selectedFiles.map(f=>f.id).indexOf(e.id)<0),
    opened:(state)=>state.opened,
    loading:(state)=>state.loading
};

const actions = {
    open({commit, dispatch}, payload){
        dispatch('fetchFiles');
        commit('setOpened', true);
        if(payload)
            commit('setOnClose',payload);
    },
    close({commit, state}){
        commit('setOpened', false);
        if(state.onClose){
            state.onClose(state.selectedFiles);
            commit('clear');
        } 
    },
    fetchFiles({commit, rootGetters, getters}){
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
    setOpened(state, payload){
        state.opened = payload;
    },
    addFile(state, payload){
        state.selectedFiles.push(payload);
        if(!state.files.some(e=>e.id == payload.id))
            state.files.push(payload);        
    },
    removeFile(state, payload){
        state.selectedFiles = state.selectedFiles.filter(e=>e.id != payload);
    },
    setLoading(state){
        state.loading = !state.loading;
    },
    clear(state){
        state.files = [];
        state.selectedFiles = [];
        state.onClose = null;
    },
    setOnClose(state, payload){
        state.selectedFiles = [...payload.selectedFiles];
        state.onClose = payload.onClose;
    }
};



export default {
    namespaced: true,
    mutations,
    getters,
    actions,
    state
}