const state=()=>({
    dialog:false,
    config:null
})

const getters = {
    dialog:state=>state.dialog,
    config:state=>state.config
}

const mutations = {
    open(state, payload){
        state.dialog = true;
        state.config = payload;
    },
    close(state){
        state.dialog = false;
        state.config = null;
    }
}

export default {
    namespaced:true,
    mutations,
    state,
    getters
}