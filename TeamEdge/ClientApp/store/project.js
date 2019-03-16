import router from '../router/index'
import axios from 'axios'

const getters = {
    role:(state)=>state.currentProj.role,
    participants:(state)=>state.currentProj.participants,

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
        router.push({name:'project', params:{id:projId}})
        commit('setLoading', true);
        axios.get(`/api/projects/${projId}`).then(
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