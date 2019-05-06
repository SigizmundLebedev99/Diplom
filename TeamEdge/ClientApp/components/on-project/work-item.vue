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
                                <v-spacer/>
                                <v-btn class="primary text-none" small @click="saveChanges()">Сохранить</v-btn>
                                <v-btn class="primary text-none ml-0" small @click="reset()">Откатить</v-btn>
                            </v-layout>
                            <v-layout wrap>
                                <v-flex xs12 md6>
                                    <v-subheader class="pl-0 mt-1 subtitle">Название</v-subheader>
                                    <v-text-field v-model="currentWI.changed.name" class="pt-0 mt-0 mr-3">
                                    </v-text-field>
                                </v-flex>
                                <v-flex>
                                    <v-layout v-if="currentWiType.assignable" column align-end>
                                        <v-subheader class="pl-0 mt-1 subtitle">Исполнитель</v-subheader>
                                        <v-layout align-center class="mr-3" v-if="assignedTo">
                                            <v-avatar color="primary" size="30">
                                                <span v-if="!assignedTo.avatar" class="white--text text-xs-center" medium>
                                                    {{`${assignedTo.fullName.split(' ').map(s=>s[0]).join('')}`}}
                                                </span>
                                                <v-img v-else :src="assignedTo.avatar"/>
                                            </v-avatar>
                                            <span class="ml-3">{{assignedTo.fullName}}</span>
                                        </v-layout>
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
                                    <router-link v-if="currentWI.changed.parent" :to="{name:currentWI.changed.parent.code, params:{number:currentWI.changed.parent.number}}">
                                        <span>{{`${currentWI.changed.parent.code}${currentWI.changed.parent.number} - ${currentWI.changed.parent.name}`}}</span>
                                    </router-link>
                                    <span v-else>Предок не выбран</span>
                                </v-flex>
                                <v-flex xs12 sm6 v-if="currentWiType.epickLink">
                                    <v-layout align-center>
                                        <v-subheader class="pl-0">Epick link</v-subheader>
                                        <wi-selector code="EPICK" @selected="epickSelected" icon="edit"/>
                                        <v-btn small icon class="ml-0" @click="dropEpick()">
                                            <v-icon small>close</v-icon>
                                        </v-btn> 
                                    </v-layout>
                                    <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                                    <router-link v-if="currentWI.changed.epick" :to="{name:currentWI.changed.epick.code, params:{number:currentWI.changed.epick.number}}">
                                        <span>{{`${currentWI.changed.epick.code}${currentWI.changed.epick.number} - ${currentWI.changed.epick.name}`}}</span>
                                    </router-link>
                                    <span v-else>Epick не выбран</span>
                                </v-flex>
                            </v-layout>
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
import {mapGetters, mapMutations} from 'vuex'
import onResize from '../../mixins/on-resize'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import adapters from '../../image-upload-adapter'
import files from './files'
import comments from './comments'
import workItems from '../../data/work-items-object'
import wiSelector from './wi-selector'
import workItemInfo from './work-item-info'

export default {
    mixins:[onResize],
    components:{
        'files':files,
        'wi-selector': wiSelector,
        'comments':comments,
        'work-item-info':workItemInfo
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
        }
    }),
    computed:{
        currentWiType(){
            return workItems[this.$route.name];
        },
        ...mapGetters({
            currentWI:'project/currentWI'
            }),
        assignedTo(){
            return this.currentWI.changed.assignedTo;
        }
    },
    methods:{
        enter(){
            if(this.currentWI){
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
                        this.loading = false;
                    },
                    r=>console.log(r.response)
                );
            }
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
        dropEpick(){
            this.currentWI.changed.epick = null;
        },
        ...mapMutations({addWI:'project/addWI'}),
        saveChanges(){
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
                r=>{this.currentWI.source = Object.assign({}, this.currentWI.changed)},
                r=>{}
            )
        }
    },
    watch:{
        currentWI(from, to){
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
</style>


