<template>
    <v-container class="mx-0">
        <v-btn small class="text-none" @click="openSprintForm()">Создать спринт</v-btn>
        <create-sprint @sprintCreated="sprintAdded"/>
        <v-layout justify-center class="mt-3" v-if="!loading && !sprints.length">
            <span class="title">Пока в проекте не создано ни одного спринта</span>
        </v-layout>
        <v-treeview :items="sprints" item-key="id">
            <template v-slot:label="{ item }">
                <div v-if="!item.code">
                    <v-layout align-center>
                        <span>Спринт {{item.number}}</span>
                        <v-btn icon small class="ml-5">
                            <v-icon small class="ml-2" @click="editScript(item)">edit</v-icon>
                        </v-btn>
                    </v-layout>
                </div>
                
                <v-layout v-else align-center>
                    <v-chip small label :color="workItems[item.code].color" class="white--text">
                        <span class="chip">{{item.code}}-{{item.number}}</span>
                    </v-chip>
                    <router-link class="ml-2" :to="{name:item.code, params:{number:item.number}}">{{item.name}}</router-link>
                </v-layout>
            </template>
        </v-treeview>
    </v-container>
</template>

<script>
import createSprint from '../create-sprint'
import workItems from '../../../data/work-items-object'
import {mapGetters, mapMutations} from 'vuex'
export default {
    components:{
        'create-sprint':createSprint
    },
    methods:{
        sprintAdded(model){
            this.sprints.push(model);
        },
        ...mapMutations({
            openSprintForm:'backlog/openSprintForm'
        }),
        editScript(script){
            
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

