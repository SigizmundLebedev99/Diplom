import Vue from 'vue'

const actions = {
    async fetchProjects({commit, rootState}){
        try{
            var responce = await Vue.http.get(`/api/project/user/${rootState.auth.getters.profile.Id}`);
            commit('setInvites', responce.data.invites);
            commit('setProjects', responce.data.projects);
        }
        catch(ex){
        }
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
    invitesCount:state => state.invites.length
}

const state = ()=>({
    invites: [],
    projects: []
})

export default {
    namespaced:true,
    actions,
    mutations,
    state
}