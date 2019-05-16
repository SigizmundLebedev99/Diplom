import axios from 'axios'

const state=()=>({
    timesheet:[],
    loading:false
})

const mutations = {
    populateTimesheet(state, payload){
        state.timesheet = payload;
    },
    setLoading(state, payload){
        state.loading = payload;
    }
}

const getters = {
    loading:state=>state.loading,
    timesheet:state=>state.timesheet
}

const actions = {
    fetchTimesheet({commit}, itemId){
        commit('setLoading', true)
        axios.get(`/api/timesheet/workitem/${itemId}`)
        .then(r=>{
            commit('populateTimesheet', r.data);
            commit('setLoading', false);
        },
        r=>console.log(r.response))
    }
}

export default {
    namespaced:true,
    actions,
    mutations,
    state,
    getters
}