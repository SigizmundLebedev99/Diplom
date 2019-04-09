import axios from 'axios'
import router from '../router/index'

const getters = {
    role:(state)=>state.currentProj.accessLevel,
    participants:(state)=>state.currentProj.participants,
    project:(state)=>state.currentProj,
    loading:(state)=>state.loading,
    workItems:(state)=>state.workItems,
    currentWI:(state, getters, rootState)=>state.workItems.find(e=>e.code===rootState.route.name && e.number == rootState.route.params.number)
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
    setWorkItems(state, payload){
        state.workItems = payload;
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
    },
    dropWI({state, rootState}, payload){  
        var flag = rootState.route.name == payload.code && rootState.route.params.number == payload.number   
        state.workItems = state.workItems.filter(e=>e !== payload);
        if(flag){
            if(state.workItems.length){
                var item = state.workItems[0];
                router.push({name:item.code, params:{number:item.number}});            
            }
            else
                router.push({name:'backlog'});
        }
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