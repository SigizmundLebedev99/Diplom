<template>
    <v-container class="mx-0 pt-0 scroll-y">
        <v-btn small @click="preWICreating()" class="ml-2 mb-4 text-none">
            <v-icon>
                add
            </v-icon>
            Создать единицу работы
        </v-btn>

        <v-treeview :items="items" item-key="descriptionId" :open="allIds">
            <template v-slot:label="{ item }">
                <v-layout align-center>
                    <v-chip small label :color="workItems[item.code].color" class="white--text">
                        <span class="chip">{{item.code}}-{{item.number}}</span>
                    </v-chip>
                    <router-link class="ml-2" :to="{name:item.code, params:{number:item.number}}">{{item.name}}</router-link>
                    <v-spacer/>
                    <v-chip label small v-if="item.sprintNumber">Спринт {{item.sprintNumber}}</v-chip>
                </v-layout>
            </template>
        </v-treeview> 

    </v-container>
</template>

<script>
import {mapActions} from 'vuex'
import workItems from '../../../data/work-items-object'
export default {
    mounted() {
        this.fetchItems();
    },
    data:()=>({
        items:[],
        allIds:[]
    }),
    methods:{
        ...mapActions({preWICreating:'createWorkItem/preWICreating'}),
        fetchItems(){
            this.$http.get(`/api/workitems/project/${this.$route.params.projId}/backlog`)
            .then(r=>this.buildTree(r.data))
        },
        buildTree(items){
            var dictionary = {};
            items.forEach(i=>{dictionary[i.descriptionId] = i; this.allIds.push(i.descriptionId);});
            items.forEach(i=>{
                if(i.parentId)
                {
                    if(dictionary[i.parentId].children)
                        dictionary[i.parentId].children.push(i);
                    else
                        dictionary[i.parentId].children = [i];
                }
            });
            this.items = items.filter(i=>!i.parentId);
        }
        
    },
    computed:{
        workItems(){
            return workItems;
        }
    }
}
</script>

<style scoped>
.chip{
    width: 75px;
    text-align: center;
}
</style>

