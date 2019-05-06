<template>
    <v-container class="mx-0">
        <create-sprint @sprintCreated="sprintAdded"/>
        <v-layout justify-center class="mt-3" v-if="!loading && !sprints.length">
            <span class="title">Пока в проекте не создано ни одного спринта</span>
        </v-layout>
        <v-treeview :items="sprints" item-key="id">
            <template v-slot:label="{ item }">
                <span v-if="!item.code">Спринт {{item.number}}</span>
                 <v-layout v-else align-center>
                    <v-chip small label :color="workItems[item.code].color" class="white--text">
                        <span class="chip">{{item.code}}-{{item.number}}</span>
                    </v-chip>
                    <router-link class="ml-2" :to="{name:item.code, params:{number:item.number}}">{{item.name}}</router-link>
                    <v-spacer/>
                 </v-layout>
            </template>
        </v-treeview>
    </v-container>
</template>

<script>
import createSprint from '../create-sprint'
import workItems from '../../../data/work-items-object'
import {mapGetters} from 'vuex'
export default {
    components:{
        'create-sprint':createSprint
    },
    methods:{
        sprintAdded(model){
            this.sprints.push(model);
        }
    },
    computed:{
        ...mapGetters({
            items:'backlog/items',
            sprints:'backlog/sprints',
            loading:'backlog/sprintsLoading'
        }),
        workItems(){
            return workItems;
        },
    }
}
</script>

<style>

</style>
