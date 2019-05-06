import axios from 'axios'

const state = ()=>({
    items : [],
    itemsTree:[],
    sprints:[],
    sprintsLoading:false,
    sprintOpened:false,
    editSprint:false
});

const mutations = {
    populateBacklogItems(state, payload){
        state.items = payload;
    },
    populateSprints(state, payload){
        state.sprints = payload;
    },
    buildTree(state, items){
        var dictionary = {};
        items.forEach(i=>dictionary[i.descriptionId] = i);
        items.forEach(i=>{
            if(i.parentId)
            {
                if(dictionary[i.parentId].children)
                    dictionary[i.parentId].children.push(i);
                else
                    dictionary[i.parentId].children = [i];
            }
        });
        state.itemsTree = items.filter(i=>!i.parentId);
    },

    setSprintsLoading(state, payload){
        state.setSprintsLoading = payload;
    },

    openSprintForm(state, edit){
        state.editSprint = edit;
        state.sprintOpened = true;
    },

    closeSprintForm(state){
        state.sprintOpened = false;
    }
};

const actions = {
    fetchItems({commit, rootState, dispatch}){
        axios.get(`/api/workitems/project/${rootState.route.params.projId}/backlog`)
            .then(r=>{
                commit('populateBacklogItems', r.data);
                commit('buildTree', r.data);
                dispatch('fetchSprints');
            })
    },
    fetchSprints({state, commit, rootState}){
        commit('setSprintsLoading', true);
        axios.get(`/api/sprints/project/${rootState.route.params.projId}`)
        .then(r=>{
                r.data.forEach(e=>{
                    e.children=state.items.filter(i=>i.sprintNumber == e.number);
                    })
                commit('populateSprints', r.data);
                commit('setSprintsLoading', false);
            },
        r=>console.log(r.response));
    }
}

const getters = {
    items:state=>state.items,
    itemsTree:state=>state.itemsTree,
    sprints:state=>state.sprints,
    sprintsLoading:state=>state.sprintsLoading
};

export default{
    namespaced:true,
    state,
    getters,
    mutations,
    actions
}