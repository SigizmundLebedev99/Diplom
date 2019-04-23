<template>
    <v-container class="mx-0 pt-0 scroll-y">
        <v-btn small @click="preWICreating()" class="ml-2 text-none">
            Создать
            <v-icon small>
                add
            </v-icon>
        </v-btn>

        <v-layout row wrap>
            <v-flex md6 xs12>
                <v-treeview :items="items" item-key="descriptionId" :open="allIds">
                    <template v-slot:label="{ item }">
                        <v-layout align-center>
                            <v-chip label :color="workItems[item.code].color" class="white--text">
                                <span class="chip">{{item.code}}-{{item.number}}</span>
                            </v-chip>
                            <router-link class="ml-2" :to="{name:item.code, params:{number:item.number}}">{{item.name}}</router-link>
                        </v-layout>
                    </template>
                </v-treeview> 
            </v-flex>
        </v-layout>
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

