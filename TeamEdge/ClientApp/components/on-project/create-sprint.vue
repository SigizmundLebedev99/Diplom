<template>
    <v-dialog v-model="dialog" :width="ofSize({xs:350, sm:400})" persistent>
        
        <v-card class="elevation-12">
            <v-toolbar dark color="primary" dense>
                <v-toolbar-title>Новый спринт</v-toolbar-title>
                <v-spacer></v-spacer>
                <v-btn icon @click="close()">
                    <v-icon>
                        close
                    </v-icon>
                </v-btn>
            </v-toolbar>
            <v-card-text>
                <v-layout align-center>
                    <v-subheader class="pl-0">Запланированная работа</v-subheader>
                    <wi-selector @selected="addChild" :forSprint="true"></wi-selector>
                </v-layout>
                <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                <span v-if="!children.length">Не выбран ни один потомок</span>
                <v-list dense style="items-list scroll-y">
                    <v-list-tile v-for="(item, i) in children" :key="i">
                    <v-layout row justify-space-between align-center>
                        <span>{{`${i+1}) ${item.code}${item.number} - ${item.name}`}}</span>
                        <v-btn small icon @click="removeChild(item.descriptionId)">
                        <v-icon small>close</v-icon>
                        </v-btn>
                    </v-layout>
                    </v-list-tile>
                </v-list>
            </v-card-text>
            <v-card-actions>
                <v-layout row justify-end>
                    <v-btn color="primary" :loading="loading" class="mr-2" @click="submit()">Создать</v-btn>
                </v-layout>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import onResize from '../../mixins/on-resize'
import wiSelector from './wi-selector'
import { mapMutations, mapGetters } from 'vuex';
const defaultModel = {
    startDate:null,
    endDate:null,
    tasks:[],
    userStories:[],
}
export default {
    components:{
        'wi-selector':wiSelector
    },
    mixins:[onResize],
    data:()=>({
        model:{...defaultModel},
        loading:false,
        children:[],
        selectionFilter:item=>!(this.children.map(e=>e.descriptionId).some(item.descriptionId))
    }),
    methods:{
        close(){
          this.model={...defaultModel};
          this.children = [];
          this.closeDialog();
        },
        addChild(item){
            this.children.push(item);
        },
        removeChild(id){
            this.children = this.children.filter(e=>e.descriptionId != id);
        },
        submit(){
            this.children.forEach(i=>{
                if(i.code=='STORY')
                    this.model.userStories.push(i.descriptionId)
                else
                    this.model.tasks.push(i.descriptionId);
            })
            this.loading = true;
            this.$http.post(`/api/sprints/project/${this.$route.params.projId}`, this.model).then(
                r=>{
                    this.loading = false;
                    this.children.forEach(e=>e.sprintNumber = r.data.number)
                    this.$emit('sprintCreated', r.data);
                    this.close();
                },
                r=>console.log(r.response)
            )
        },
        ...mapMutations({
            closeDialog:'backlog/closeSprintForm'
        })
    },
    computed:{
        ...mapGetters({
            dialog:'backlog/sprintOpened'
        })
    }
}
</script>

<style scoped>
.items-list{
    height: 150px;
}
</style>

