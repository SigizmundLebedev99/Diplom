import axios from 'axios'

const actions = {
    fetchProjects({commit, rootState}){
        axios.get(`/api/project/user/${rootState.auth.profile.userId}`)
        .then(response => {
            commit('setInvites', response.data.invites);
            commit('setProjects', response.data.projects);
        }, response => {
            console.log(response),
            console.log(response.response)
        });   
    }
}

const mutations = {
    setInvites(state, payload){
        state.invites = payload;
    },
    setProjects(state, payload){
        state.projects = payload;
    }
}

const getters = {
    projects: state=> state.projects,
    invites: state=>state.invites
}

const state = ()=>({
    invites: [],
    projects: []
})

export default {
    namespaced:true,
    actions,
    mutations,
    state,
    getters
}