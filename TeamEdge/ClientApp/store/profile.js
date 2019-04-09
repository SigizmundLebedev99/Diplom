import router from '../router/index'
import axios from 'axios'

const state = ()=>({
    profile:{},
    loading:false,
});

const mutations = {
    setDefaults(state, payload){
        if(payload)
            state.profile = Object.assign({}, payload);
        else
            state.profile = null;
    },
    setLoading(state){
        state.loading = !state.loading;
    }
}

const getters = {
    profile: state => state.profile,
    loading: state => state.loading
}

const actions = {
    open({rootGetters, commit, rootState}, userId){
        if(!userId)
            userId = rootGetters['auth/profile'].userId
        router.push({name:'profile'});
        commit('setLoading');
        axios.get(`/api/account/info/${userId}`).then(
        r=>{
            commit('setLoading');
            commit('setDefaults', r.data);
        },
        r=>{
            commit('setLoading');
            console.log(r.response);
        })
    }
}

export default{
    namespaced:true,
    state,
    getters,
    actions,
    mutations
}