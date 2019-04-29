<template>
    <v-container class="mx-0">
        <create-sprint @sprintCreated="fetchSprints()"/>
        <v-layout justify-center class="mt-3" v-if="!loading && !sprints.length">
            <span class="title">Пока в проекте не создано ни одного спринта</span>
        </v-layout>
        <v-treeview :items="sprints" item-key="id">
            <template v-slot:label="{ item }">
                <span>Спринт {{item.number}}</span>
            </template>
        </v-treeview>
    </v-container>
</template>

<script>
import createSprint from '../create-sprint'
export default {
    components:{
        'create-sprint':createSprint
    },
    mounted(){
        this.fetchSprints();
    },
    data:()=>({
        sprints:[],
        loading:false
    }),
    methods:{
        fetchSprints(){
            this.$http.get(`/api/sprints/project/${this.$route.params.projId}`)
            .then(r=>{
                r.data.forEach(e=>e.children=[])
                this.sprints = r.data
                },
            r=>console.log(r.response));
        }
    }
}
</script>

<style>

</style>
