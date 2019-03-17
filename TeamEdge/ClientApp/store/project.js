import axios from 'axios'

const getters = {
    role:(state)=>state.currentProj.role,
    participants:(state)=>state.currentProj.participants,
    project:(state)=>state.currentProj,
    loading:(state)=>state.loading
};

const mutations = {
    setProject(state, payload){
        state.currentProj = payload;
    },
    setLoading(state, payload){
        state.loading = payload
    }
};

const actions = {
    fetchProject({commit}, projId){
        commit('setLoading', true);
        axios.get(`/api/project/${projId}`).then(
            r=>{
                commit('setProject', r.data);
                commit('setLoading', false);
            },
            r=>{
            }
        )
    }
};

const state = ()=>({
    currentProj:{
        id:null,
        role:null,
        participants:[]
    },
    currentTasks:[],
    loading:false
});

export default {
    namespaced: true,
    mutations,
    getters,
    state,
    actions
}