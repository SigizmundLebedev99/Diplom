<template>
    <v-container>
        <p v-if="!itemType">Выберите тип единицы работы</p>
        <div v-else>
            <div v-if="itemType.parent">
                <v-layout align-center>
                    <v-subheader class="pl-0">Предок</v-subheader>
                    <wi-selector :code="itemType.parent" @selected="parentSelected" icon="edit"></wi-selector>
                    <v-btn small icon class="ml-0" @click="dropParent()" v-if="!itemType.requireParent">
                        <v-icon small>close</v-icon>
                    </v-btn> 
                    <sub v-if="itemType.requireParent">Необходимо выбрать предка</sub>
                </v-layout>
                <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                <span v-if="currentParent">{{`${currentParent.code}${currentParent.number} - ${currentParent.name}`}}</span>
                <span v-else>Предок не выбран</span>
            </div>
            <div v-if="itemType.epicLink && !currentParent" class="mt-3">
                <v-layout align-center>
                    <v-subheader class="pl-0">Epic link</v-subheader>
                    <wi-selector code="EPIC" @selected="epicSelected" icon="edit"></wi-selector>
                    <v-btn small icon class="ml-0" @click="dropEpicLink()">
                        <v-icon small>close</v-icon>
                    </v-btn>
                </v-layout>
                <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                <span v-if="epic">{{`${epic.code}${epic.number} - ${epic.name}`}}</span>
                <span v-else>Epic не выбран</span>
            </div>
            <div v-if="itemType.children" class="mt-3">
              <v-layout align-center>
                <v-subheader class="pl-0">Потомки</v-subheader>
                <wi-selector :code="itemType.children" @selected="addChild"></wi-selector>
              </v-layout>
              <v-divider class="mr-3 mt-0 mb-2"></v-divider>
              <span v-if="!children.length">Не выбран ни один потомок</span>
              <v-list dense>
                <v-list-tile v-for="(item, i) in children" :key="i">
                  <v-layout row justify-space-between align-center>
                    <span>{{`${i+1}) ${item.code}${item.number} - ${item.name}`}}</span>
                    <v-btn small icon @click="removeChild(item.descriptionId)">
                      <v-icon small>close</v-icon>
                    </v-btn>
                  </v-layout>
                </v-list-tile>
              </v-list>
            </div>
        </div>
    </v-container>
</template>

<script>
import {mapMutations, mapGetters} from 'vuex'
import wiSelector from './wi-selector'
export default {
    components:{
        'wi-selector': wiSelector
    },
    props:[
        'itemType'
    ],
    data:()=>({
        parent:null,
        children:[],
        epic:null,
    }),
    methods:{
        ...mapMutations({
            setAdditionalInfo:'createWorkItem/setAdditionalInfo'
        }),
        parentSelected(item){
            this.additionalInfo.parentId = item.descriptionId;
            this.parent = item;
        },
        dropParent(){
            this.additionalInfo.parentId = null;
            this.predefined.parent = null
            this.parent = null;
        },
        epicSelected(item){
            this.epic = item;
            this.additionalInfo.epicId = item.descriptionId;
        },
        dropEpicLink(){
            this.epic = null;
            this.additionalInfo.epicId = null;
        },
        addChild(item){
            if(!this.additionalInfo.childrenIds)
                this.additionalInfo.childrenIds = [];
            var arr = [];
            if(this.additionalInfo.childrenIds.some(id=>id==item.descriptionId))
                return;
            if(this.itemType.code == 'EPIC' && item.code != 'STORY'){       
                if(!this.additionalInfo.linkIds)
                    this.additionalInfo.linkIds = [];
                if(this.additionalInfo.linkIds.some(id=>id==item.descriptionId))
                    return;
                this.additionalInfo.linkIds.push(item.descriptionId)              
            }
            else
                this.additionalInfo.childrenIds.push(item.descriptionId);
            this.children.push(item);
        },
        removeChild(id){
            this.children = this.children.filter(e=>e.descriptionId != id);
            this.additionalInfo.childrenIds = this.children.map(e=>e.descriptionId);
            if(this.itemType.code == 'EPIC')
                this.additionalInfo.linkIds = this.additionalInfo.linkIds.filter(e=>e!=id);
        },
        clear(){
            this.setAdditionalInfo({});
            this.parent = null;
            this.epic = null;
            this.children = [];
            this.predefined.parent = null;
        }
    },
    computed:{
        ...mapGetters({
            additionalInfo:'createWorkItem/additionalInfo',
            predefined:'createWorkItem/predefined'
        }),
        currentParent(){
            return this.parent || this.predefined.parent;
        }
    },
    watch:{
        additionalInfo(){
            if(this.additionalInfo.parent){
                this.parent = this.additionalInfo.parent;
                this.additionalInfo.parentId = this.parent.descriptionId
            }
        }
    }
}
</script>
