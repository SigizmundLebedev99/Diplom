import axios from 'axios'

const getters = {
    role:(state)=>state.currentProj.role,
    participants:(state)=>state.currentProj.participants,
    project:(state)=>state.currentProj,
    loading:(state)=>state.loading,
    workItems:(state)=>state.workItems
};

const mutations = {
    setProject(state, payload){
        state.currentProj = payload;
    },
    setLoading(state, payload){
        state.loading = payload
    },
    addWI(state, payload){
        state.workItems.push(payload);
    },
    dropWI(state, payload){
        state.workItems = state.workItems.filter(e=>e.descriptionId !== payload);
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
    },
    loadWorkItem(){
        commit('setLoading', true);
        axios.get(`/api/workitems/project/${projId}/item`).then(
            r=>{
                commit('addWI', r.data);
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
    loading:false,
    workItems:[]
});

export default {
    namespaced: true,
    mutations,
    getters,
    state,
    actions
}