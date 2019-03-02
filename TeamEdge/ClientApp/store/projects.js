import Vue from 'vue'

const actions = {
    async fetchData({commit, rootState}){
        try{
            console.log(rootState.auth.getters.profile.Id);
            var responce = await Vue.http.get(`/api/project/user/${rootState.auth.getters.profile.Id}`);
            commit('setInvites', responce.body.invites);
            commit('setProjects', responce.body.projects);
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

const state = ()=>({
    invites: [],
    projects: []
})

export default {
    namespaced:true,
    mutations,
    state
}