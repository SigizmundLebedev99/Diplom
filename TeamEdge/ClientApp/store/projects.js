import axios from 'axios'

const actions = {
    fetchProjects({commit, rootState}){
        commit('setLoading', true);
        axios.get(`/api/project/user/${rootState.auth.profile.userId}`)
        .then(response => {
            commit('setInvites', response.data.invites);
            commit('setProjects', response.data.projects);
            commit('setLoading',false);
        }, response => {
            commit('setLoading',false);
        });   
    }
}

const mutations = {
    setInvites(state, payload){
        state.invites = payload;
    },
    setProjects(state, payload){
        state.projects = payload;
    },
    setLoading(state, payload){
        state.loading = payload;
    }
}

const getters = {
    projects: state=> state.projects,
    invites: state=>state.invites,
    loading: state=>state.loading
}

const state = ()=>({
    invites: [],
    projects: [],
    loading: false
})

export default {
    namespaced:true,
    actions,
    mutations,
    state,
    getters
}