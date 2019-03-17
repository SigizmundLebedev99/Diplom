const state = ()=>({
    dialog:false,
    predefinedCode:null
});

const getters = {
    dialog:(state)=>state.dialog
}

const mutations = {
    setDialog(state, {dialog,predefinedCode}){
        state.dialog = dialog;
        if(!dialog)
            state.predefinedCode = null;
        else
            if(predefinedCode)
                state.predefinedCode = predefinedCode;
    }
}

export default{
    namespaced:true,
    state,
    getters,
    mutations
}