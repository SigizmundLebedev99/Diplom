<template>
    <div>
        <v-container v-if="loading">
            <v-layout column justify-center align-center fill-height>
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-layout>
        </v-container>
        <div v-else>
            <v-layout row justify-end wrap fill-height>
                <v-flex md8 sm12>
                    <v-toolbar dark :class="currentWiType.color" class="mainheader" dense flat>
                        <v-toolbar-title class="ml-2">
                            {{currentWiType.name}}-{{currentWI.number}} "{{currentWI.source.name}}"
                        </v-toolbar-title>
                    </v-toolbar>
                    <v-container class="pt-0 divide">
                        <v-layout column>
                            <v-layout align-center>
                                <work-item-info/>
                                <div class="status" v-if="status">
                                    <v-select class="ml-3"
                                        v-model="status"
                                        :items="statuses"
                                        @change="changeStatus"
                                        required>
                                        <template v-slot:selection="data">
                                            <span>{{data.item.value}}</span>
                                        </template>
                                        <template v-slot:item="{ index, item }">
                                            <span>{{item.value}}</span>
                                        </template>
                                    </v-select>
                                </div>
                                <v-spacer/>
                                <v-btn :loading="updateLoading" class="primary text-none" small @click="saveChanges()">Сохранить</v-btn>
                                <v-btn class="primary text-none ml-0" small @click="reset()">Откатить</v-btn>
                                <v-btn icon small @click="load()">
                                    <v-icon>
                                        refresh
                                    </v-icon>
                                </v-btn>
                            </v-layout>
                            <v-divider/>
                            <v-layout wrap>
                                <v-flex xs12 md6>
                                    <v-subheader class="pl-0 mt-1 subtitle">Название</v-subheader>
                                    <v-text-field v-model="currentWI.changed.name" class="pt-0 mt-0 mr-3">
                                    </v-text-field>
                                </v-flex>
                                <v-flex>
                                    <v-layout v-if="currentWiType.assignable" column align-end>
                                        <v-layout>
                                            <v-subheader class="pl-0 mt-1 subtitle">Исполнитель</v-subheader>
                                            <user-selector @userSelected="userSelected"/>
                                            <v-btn class="ml-0" icon small @click="currentWI.changed.assignedTo = null">
                                                <v-icon small>close</v-icon>
                                            </v-btn>
                                        </v-layout>
                                        <v-layout align-center class="mr-3" v-if="assignedTo">
                                            <v-avatar color="primary" size="30">
                                                <span v-if="!assignedTo.avatar" class="white--text text-xs-center" medium>
                                                    {{`${assignedTo.fullName.split(' ').map(s=>s[0]).join('')}`}}
                                                </span>
                                                <v-img v-else :src="assignedTo.avatar"/>
                                            </v-avatar>
                                            <span class="ml-3">{{assignedTo.fullName}}</span>
                                        </v-layout>
                                        <span v-else class="mr-3">Исполнитель не назначен</span>
                                    </v-layout>
                                </v-flex>
                            </v-layout>
                            <v-subheader class="pl-0 subtitle">Описание</v-subheader>
                            <ckeditor :editor="editor" v-model="currentWI.changed.description.descriptionText" :config="editorConfig"></ckeditor>
                            <v-layout row wrap>
                                <v-flex xs12 sm6 v-if="currentWiType.parent">
                                    <v-layout align-center>
                                        <v-subheader class="pl-0">Предок</v-subheader>
                                        <wi-selector :code="currentWiType.parent" @selected="parentSelected" icon="edit"/>
                                        <v-btn small icon class="ml-0" @click="dropParent()">
                                            <v-icon small>close</v-icon>
                                        </v-btn> 
                                    </v-layout>
                                    <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                                    <v-layout align-center v-if="currentWI.changed.parent">
                                        <v-chip small label :color="workItems[currentWI.changed.parent.code].color" class="white--text ml-0">
                                            <span class="chip">{{currentWI.changed.parent.code}}-{{currentWI.changed.parent.number}}</span>
                                        </v-chip>
                                        <router-link class="ml-2" :to="{name:currentWI.changed.parent.code, params:{number:currentWI.changed.parent.number}}">{{currentWI.changed.parent.name}}</router-link>
                                    </v-layout>
                                    <span v-else>Предок не выбран</span>
                                </v-flex>
                                <v-flex xs12 sm6 v-if="currentWiType.epickLink">
                                    <v-layout align-center>
                                        <v-subheader class="pl-0">Epic link</v-subheader>
                                        <wi-selector code="EPIC" @selected="epickSelected" icon="edit"/>
                                        <v-btn small icon class="ml-0" @click="dropEpic()">
                                            <v-icon small>close</v-icon>
                                        </v-btn>
                                    </v-layout>
                                    <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                                    <v-layout align-center v-if="currentWI.changed.epick">
                                        <v-chip small label :color="workItems['EPIC'].color" class="white--text ml-0">
                                            <span class="chip">{{currentWI.changed.epick.code}}-{{currentWI.changed.epick.number}}</span>
                                        </v-chip>
                                        <router-link class="ml-2" :to="{name:currentWI.changed.parent.code, params:{number:currentWI.changed.parent.number}}">{{currentWI.changed.parent.name}}</router-link>
                                    </v-layout>
                                    <span v-else>Epic не выбран</span>
                                </v-flex>
                            </v-layout>
                            <div v-if="currentWiType.children" class="mt-3">
                                <v-layout align-center>
                                    <v-subheader class="pl-0">Потомки</v-subheader>
                                    <v-spacer/>
                                    <v-btn small class="text-none primary" @click="createChild()">
                                        <v-icon small>add</v-icon>
                                        Создать
                                    </v-btn>
                                </v-layout>
                                <v-divider class="mb-2"/>
                                <span v-if="!currentWI.changed.children.length">Нет дочерних единиц работы</span>
                                <v-layout align-center v-for="(t,i) in currentWI.changed.children" :key="i">
                                    <v-chip small label :color="workItems[t.code].color" class="white--text ml-0">
                                        <span class="chip">{{t.code}}-{{t.number}}</span>
                                    </v-chip>
                                    <router-link class="ml-2" :to="{name:t.code, params:{number:t.number}}">{{t.name}}</router-link>
                                </v-layout>
                            </div>
                        </v-layout>  
                    </v-container>       
                </v-flex>
                <v-flex md4 sm12>
                    <v-tabs dark
                        :color="currentWiType.color"
                        v-model="model"
                        centered
                        slider-color="yellow">
                        <v-tab ripple class="text-none">
                            Комментарии
                        </v-tab>
                        <v-tab ripple class="text-none">
                            Вложения
                        </v-tab>
                        <v-tab ripple class="text-none">
                            История
                        </v-tab>
                    </v-tabs>
                    <div>
                        <v-tabs-items v-model="model">
                            <v-tab-item>
                                <comments/>                         
                            </v-tab-item>
                            <v-tab-item>
                                <files></files>
                            </v-tab-item>
                            <v-tab-item>
                                <v-card flat>
                                <v-card-text>История</v-card-text>
                                </v-card>
                            </v-tab-item>
                        </v-tabs-items>
                    </div>
                </v-flex>
            </v-layout>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapMutations, mapActions} from 'vuex'
import onResize from '../../mixins/on-resize'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import adapters from '../../image-upload-adapter'
import files from './files'
import comments from './comments'
import workItems from '../../data/work-items-object'
import wiSelector from './wi-selector'
import workItemInfo from './work-item-info'
import userSelector from './user-selector'

import statuses from '../../data/statuses'

export default {
    mixins:[onResize],
    components:{
        'files':files,
        'wi-selector': wiSelector,
        'comments':comments,
        'work-item-info':workItemInfo,
        'user-selector':userSelector
    },
    mounted(){
        this.enter();
    },
    data:()=>({
        loading:true,
        model:null,
        editor: ClassicEditor,
        editorConfig: {
            extraPlugins: [ adapters.updateWIAdapter ]
        },
        status:{},
        updateLoading:false
    }),
    computed:{
        number(){
            return this.$route.params.number;
        },
        currentWiType(){
            return workItems[this.$route.name];
        },
        ...mapGetters({
            currentWI:'project/currentWI'
            }),
        assignedTo(){
            return this.currentWI.changed.assignedTo;
        },
        statuses(){
            return statuses.array;
        },
        workItems(){
            return workItems;
        }
    },
    methods:{
        enter(){
            if(this.currentWI){
                this.status = statuses.array[this.currentWI.source.status]
                this.loading=false;
            }
            else{
                this.loading = true;
                this.$http.get(`/api/workitems/project/${this.$route.params.projId}/item/${this.$route.name}/${this.number}`)
                .then(
                    r=>{
                        var wi = {};
                        wi.name = r.data.name;
                        wi.number = r.data.number;
                        wi.code = r.data.code;
                        wi.source = r.data;
                        wi.descriptionId = r.data.descriptionId;
                        wi.changed = Object.assign({}, r.data);
                        this.addWI(wi);
                        this.status = statuses.array[wi.source.status]
                        this.loading = false;
                    },
                    r=>console.log(r.response)
                );
            }
        },
        load(){
            this.loading = true;
            this.$http.get(`/api/workitems/project/${this.$route.params.projId}/item/${this.$route.name}/${this.number}`)
            .then(
                r=>{
                    this.currentWI.source = r.data;
                    this.currentWI.changed = Object.assign({}, r.data);
                    this.currentWI.name = r.data.name;
                     this.loading = false;
                },
                r=>console.log(r.response)
            );
        },
        reset(){
            this.currentWI.changed = Object.assign({},this.currentWI.source);
        },
        userSelected(user){
            this.currentWI.changed.assignedTo = user;
        },
        parentSelected(item){
            this.currentWI.changed.parent = item;
        },
        dropParent(){
            this.currentWI.changed.parent = null;
        },
        epickSelected(item){
            this.currentWI.changed.epick = item;
        },
        dropEpic(){
            this.currentWI.changed.epick = null;
        },
        ...mapMutations({addWI:'project/addWI'}),
        saveChanges(){
            this.updateLoading = true;
            var model = Object.assign({},this.currentWI.changed);
            model = Object.assign(model,this.currentWI.changed.description);
            model.fileIds = model.files.map(f=>f.id);
            delete model.files;
            if(model.assignedTo)
            {
                model.assignedToId = this.currentWI.changed.assignedTo.id;
                delete model.assignedTo
            }
            if(model.children)
            {
                model.childrenIds = model.children.map(e=>e.descriptionId);
                delete model.children
            }
            if(model.parent)
            {
                model.parentId = this.currentWI.changed.parent.descriptionId;
                delete model.parent;
            }
            if(model.epick){
                model.epickId = model.epick.descriptionId;
                delete model.epick;
            }
            model.projectId = this.$route.params.projId;
            this.$http.put(`/api/workitems/number/${this.currentWI.number}`,model)
            .then(
                r=>{
                    this.updateLoading = false;
                    this.currentWI.source = Object.assign({}, this.currentWI.changed)
                },
                r=>{
                    this.updateLoading = false;
                    console.log(r.response.data);
                }
            )
        },
        changeStatus(smt){
            this.$http.post(`/api/timesheet/status`,
            {
                projectId:this.$route.params.projId,
                workItemId:this.currentWI.descriptionId,
                status: statuses.object[smt]
            }).then(r=>{
                this.currentWI.source.status = this.status.key;
            },
                r=>{console.log(r.response)}
            );
        },
        createChild(){
            var predifined = {
                parentCode:this.currentWI.code,
                parent:{
                    code:this.currentWI.code,
                    number:this.currentWI.number,
                    descriptionId:this.currentWI.descriptionId,
                    name:this.currentWI.name
                }
            }
            this.preWiCreating(predifined);
        },
        ...mapActions({
            preWiCreating:'createWorkItem/preWICreating'
        })
    },
    watch:{
        currentWI(){
            this.enter();
        }
    }
}
</script>

<style scoped>
.divide{
    border-right: 1px solid #bbbbbb;
}
.subtitle{
    height: 30px;
}
.mainheader{
    transition: none;
}
.status{
    width:90px;
}
</style>


